using SentimentAnalysisConsoleApp.DataStructures;
using System;
using System.Windows;
using System.Windows.Media;
using System.Timers;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace PTwitchCapture
{

    public partial class MainWindow : Window
    {
        bool initialized = false;
        int countP1 = 0;
        int countP2 = 0;
        int countForPrintdata = 0;
        bool fisrtClick = false, firstTime = false;

        Bot bot;
        Analysis1 a1 = new Analysis1();
        Analysis2 a2 = new Analysis2();
        Boolean isPause = false;
        //-----------------------------
        //get these data from FightingICE
        int hp_p1 = 0;
        int hp_p2 = 0;
        //-------------------------------
        Timer myTimer;//time thread for calculation
        static volatile bool isRunning;
        //-----------------------------
        bool isOneSideMode = false;
        Timer oneSide_Timer;//time thread for calculation
        int oneSide_numParticipant = 10;
        double oneSide_avgMsg_perMinParticipant = 3;
        int oneSide_generateEvery = 2000;//ms
        bool oneSide_nextDirection = true;// True: p2+, False: p1-



        public MainWindow()
        {
            InitializeComponent();
            initialized = true;
            countP1 = 0;
            countP2 = 0;
            firstTime = false;
            
            TheTool.Sys = this;
            bot = new Bot(this);
            bot.createClient();
            string path1 = path_root + "/p1HP.txt";
            string path2 = path_root + "/p2HP.txt";
            int h1 = TheTool.getInt(TheTool.read_File_get1String(path1));
            int h2 = TheTool.getInt(TheTool.read_File_get1String(path2));
            hp_p1 = h1;
            hp_p2 = h2;
            applySetting();
            initThread();

            oneSideMode_setUp();

            this.Closing += new System.ComponentModel.CancelEventHandler(Window4_Closing);
            
        }

        

     
        public void getMsg(string user, string msg)
        {
            //To handle GUI by Thread
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (isPause) { return; }
                Console.WriteLine("NewMsg: " + msg);
                addTxtMsg(user, msg);
                if (checkV2.IsChecked.Value) { processV2_part1(msg); }
                else { processV1(msg); }
                //We don't use V1 recently
            }));
        }

        //simulate Mock Msg
        public void getMsg_autoP2(string msg)
        {
            //To handle GUI by Thread
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (isPause) { return; }
                //Console.WriteLine("Mock: " + msg);
                //addTxtMsg(user, msg);
                if (checkV2.IsChecked.Value) { processV2_part1_autoP2(msg); }
                else { processV1(msg); }
                //We don't use V1 recently
            }));
        }



        public void addTxtMsg(string user, string msg)
        {
            string pre = "[" + TheTool.getTime2() + "] ";
            string txt = user + ": " + msg;
            addTxt(pre + txt, true);
            collectDataMessage(user, msg, TheTool.getTime2());
        }

        public void addTxt(string s, Boolean newLine)
        {
            txtLog.Text += s;
            if (newLine) { txtLog.Text += Environment.NewLine; }
            autoScroll();
        }

        //--------------------------

        void processV1(string msg)
        {
            String result = a1.analyzeMsg(msg, checkSentiment.IsChecked.Value);
            if (result != "")
            {
                addTxt(result, true);
                updateGUI();
                exportFTG();
            }
        }

        //Part 1 invoked by Msg
        void processV2_part1(string msg)
        {
            
            if (isOneSideMode) {
                a2.addMsg(msg, isOneSideMode);
    
                countExport(); //for P1 vs P2
            }
            else
            {
                a2.addMsg(msg, isOneSideMode);
                countExport(); //for P1 vs P2
            }
        }
        void processV2_part1_autoP2(string msg)
        {
            a2.addMsg(msg, false);

            //countExport(); //for P1 vs P2
        }

        //Part 2 runs every interval
        void processV2_part2()
        {
            a2.doUpdate();
            updateGUI();
            exportFTG();
        }

        //--------------------------

        public void updateGUI()
        {
            if (checkV2.IsChecked.Value)
            {
                slide_Bal.Value = a2.bal_gui;
                txtVar.Text = a2.getMsgVar();
                progress_P1.Value = a2.f_p1_gui;
                progress_P2.Value = a2.f_p2_gui;
            }
            else
            {
                slide_Bal.Value = a1.bal_gui;
                txtVar.Text = a1.getMsgVar();
                progress_P1.Value = a1.f_p1_gui;
                progress_P2.Value = a1.f_p2_gui;
            }
            //--------------------------------------
            if (progress_P1.Value < 0)
            {
                progress_P1.Foreground = Brushes.Red;
                txtP1.Foreground = Brushes.Red;
            }
            else if (progress_P1.Value > 0)
            {
                progress_P1.Foreground = Brushes.Lime;
                txtP1.Foreground = Brushes.Lime;
            }
            else
            {
                progress_P1.Foreground = Brushes.Gray;
                txtP1.Foreground = Brushes.Gray;
            }
            if (progress_P2.Value < 0)
            {
                progress_P2.Foreground = Brushes.Red;
                txtP2.Foreground = Brushes.Red;
            }
            else if (progress_P2.Value > 0)
            {
                progress_P2.Foreground = Brushes.Lime;
                txtP2.Foreground = Brushes.Lime;
            }
            else
            {
                progress_P2.Foreground = Brushes.Gray;
                txtP2.Foreground = Brushes.Gray;
            }
        }

        //------------------------------------

        //setting
        string path_root = "file";
        string path_rank = @"C:\Users\maili\Desktop\ShareFolder\Ranking\";
        public void showError(string s, Boolean b) { Console.WriteLine("EX " + s); }
        public void showError(string s) { Console.WriteLine("EX " + s); }
        public void showError(Exception ex) { Console.WriteLine("EX " + ex.ToString()); }

        public void countExport()
        {
            string path3 = path_root + "/countP1.txt";
            countP1++;
            string a = countP1.ToString();
            TheTool.writeFile(path3, a, true);

        }

        void exportFTG()
        {
            if (checkFTG.IsChecked.Value)
            {
                string path1 = path_root + "/N.txt";
                string a = Math.Round(a2.f_p1, 2) + Environment.NewLine + "0";
                TheTool.writeFile(path1, a, true);
            }
            if (checkFTG2.IsChecked.Value)
            {
                string path2 = path_root + "/P.txt";
                string b = Math.Round(a2.f_p2, 2) + Environment.NewLine + "0";
                TheTool.writeFile(path2, b, true);
            }
            //Console.WriteLine("P1=" + a2.f_p1 + " P2=" + a2.f_p2);
        }

        void autoScroll()
        {
            txtLog.Focus();
            txtLog.CaretIndex = txtLog.Text.Length;
            txtLog.ScrollToEnd();
        }

        private void ButApply_Click(object sender, RoutedEventArgs e)
        {
            applySetting();
            oneSideMode_setUp();
        }

        void applySetting()
        {
            try
            {
                if (txtFile.Text != "") { path_root = txtFile.Text; }
                else { txtFile.Text = path_root; }
                TheTool.folder_CreateIfMissing(path_root);
            }
            catch { }
        }

        private void butTune_Click(object sender, RoutedEventArgs e)
        {
            a1.reset();
            a2.reset();
            updateGUI();
        }

        private void sent_but_Click(object sender, RoutedEventArgs e)
        {
            SentimentPrediction resultprediction = SentimentAnalyzer.predict(sent_input.Text);
            String txt = $"Text: {sent_input.Text} | Prediction: {(Convert.ToBoolean(resultprediction.Prediction) ? "Toxic" : "Non Toxic")} sentiment | Probability of being toxic: {resultprediction.Probability} ";
            sent_output.Text += txt + Environment.NewLine;
        }

        //===============================

        public void initThread()
        {
            myTimer = new Timer();
            myTimer.Interval = 1000;
            myTimer.Elapsed += myTimer_Elapsed;
            myTimer.Start();
        }


        Boolean botStarted = false;
        private void myTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //if (botStarted) { bot.createClient(); }
            if (isRunning) return;
            isRunning = true;
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    processV2_part2();
                
                    readHPfile();
                    collectData2();
                }));
                if (isPause) { return; }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Timer: " + ex.ToString());
            }
            finally { isRunning = false; }
        }

        //==========================================================================================
        Boolean dataCollect_on = false;
        Boolean dataCollect_autostart = true;
        int i_d = 0;//index of data
        List<Data2> list_data = new List<Data2>();
        List<Data_Message> list_data_msg = new List<Data_Message>();
        List<TwitchCountData> countData = new List<TwitchCountData>();

        //public static string path_saveFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\file";
        public static string path_saveFolder = "file";



        void clearData()
        {
            i_d = 0;
            list_data.Clear();
            list_data_msg.Clear();
            countData.Clear();
            updateLogOfData();
        }

        void collectData2()
        {
            if (!dataCollect_on) { return; }
            Data2 d = new Data2()
            {
                i = i_d,
                time = TheTool.getTime2(),
                dominance = a2.dominance,
                f_p1 = a2.f_p1,
                f_p2 = a2.f_p2
            };
            for (int i = 0; i < 3; i++)
            {
                d.score_pos[i] = a2.score_pos[i];
                d.score_neg[i] = a2.score_neg[i];
            }
            d.hp_p1 = hp_p1;
            d.hp_p2 = hp_p2;
            list_data.Add(d);
            i_d++;
            updateLogOfData();
        }

        void collectDataMessage(string user0, string msg0, string time0)
        {
            if (dataCollect_autostart && !dataCollect_on) { checkData_on.IsChecked = true; }
            if (!dataCollect_on) { return; }
            String timetxt = TheTool.getTime2();
            //-----------------------------
            Data_Message dm = new Data_Message()
            {
                i = i_d,
                time = time0,
                user = user0,
                msg = msg0
            };
            list_data_msg.Add(dm); 
        }

        int itest = 0;
        public void groupAndcountUser()
        {
            List<string> list = new List<String>();
            List<string> plist = new List<String>();
            plist.Add("Name,Score");
            list.Add("Scoreboard");
            list.Add("=======================");
            var GroupUser = from twitchID in list_data_msg
                            group twitchID by twitchID.user into twitchIDGroup
                            select new
                            {
                                ID = twitchIDGroup.Key,
                                Count = twitchIDGroup.Count(),
                            };

            GroupUser = GroupUser.OrderByDescending(x => x.Count).ToList();

            foreach (var item in GroupUser)
            {

                String t = "Name: " + item.ID + ", Score: " + item.Count;
                String pt = item.ID + "," + item.Count;


                list.Add(t);
                plist.Add(pt);

            }
            list.Add("=======================");
            list.Add("See your ranking: https://drive.google.com/file/d/1GhTUtZX4aEXjhY-BznR0RqYfVtY3lDTE/view?usp=sharing");
            Bot.botSpeakListGlobal("ligoligo12", list);
            string path = path_rank + "Ranking.csv";
            TheTool.exportCSV_orTXT(path, plist, false);

        }

        List<String> convertData2()
        {
            List<string> list = new List<String>();
            list.Add("id,time,dominance,pos,pos1,pos2,neg,neg1,neg2,f_p1,f_p2,hp1,hp2");
            foreach (Data2 d in list_data)
            {
                String t = d.i + "," + d.time + "," + d.dominance + "," +
                    d.score_pos[0] + "," + d.score_pos[1] + "," + d.score_pos[2] + "," +
                    d.score_neg[0] + "," + d.score_neg[1] + "," + d.score_neg[2] + "," +
                    d.f_p1 + "," + d.f_p2 + "," +
                    d.hp_p1 + "," + d.hp_p2;
                list.Add(t);
            }
            return list;
        }

        List<String> convertDataMessage()
        {
            List<string> list = new List<String>();
            list.Add("id,time,user,msg");
            foreach (Data_Message d in list_data_msg)
            {
                String t = d.i + "," + d.time + "," + d.user + "," + d.msg;
                list.Add(t);
            }
            return list;
        }

        
        void updateLogOfData()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                int t = (i_d + 1);
                String txt = "Total Sec: " + t + Environment.NewLine +
                "Messages: " + list_data_msg.Count;
                txtLog_Data.Text = txt;

            }));
        }

        //----------

        void checkFolderExist()
        {
            if (!Directory.Exists(path_saveFolder))
            {
                TheTool.folder_CreateIfMissing(path_saveFolder);
            }
        }

        void exportData(Boolean showTxt)
        {
            try
            {
                checkFolderExist();
                String filename_prefix = path_saveFolder + @"\" + TheTool.getTime();

                if (list_data.Count > 0)
                {
                    String filename1 = filename_prefix + @"_D.csv";
                    TheTool.exportCSV_orTXT(filename1, convertData2(), false);
                }
                if (list_data_msg.Count > 0)
                {
                    String filename1 = filename_prefix + @"_M.csv";
                    TheTool.exportCSV_orTXT(filename1, convertDataMessage(), false);
                }
                if (showTxt) { System.Windows.MessageBox.Show(@"Save to '" + path_saveFolder + "'", "Export CSV"); }
            }
            catch (Exception ex) { showError(ex); }
        }

        private void butData_reset_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }

        private void butData_export_Click(object sender, RoutedEventArgs e)
        {
            exportData(true);
        }

        private void checkData_on_Checked(object sender, RoutedEventArgs e)
        {
            dataCollect_on = checkData_on.IsChecked.Value;
        }

        private void butData_open_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                checkFolderExist();
                Process.Start(path_saveFolder);
            }
            catch (Exception ex) { showError(ex); }
        }

        private void butData_stop_Click(object sender, RoutedEventArgs e)
        {
            checkData_on.IsChecked = false;
            dataCollect_on = false;
            checkData_autoStart.IsChecked = false;
            dataCollect_on = false;
        }

        private void checkData_autoStart_Checked(object sender, RoutedEventArgs e)
        {
            dataCollect_autostart = checkData_autoStart.IsChecked.Value;
        }

        void Window4_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you really want to do that?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            if (result == MessageBoxResult.Yes)
            {
                e.Cancel = false;
                exportData(true);
                oneSide_Timer.Stop();
                oneSide_Timer.Dispose();
                myTimer.Stop();
                myTimer.Dispose();

            }
        }

        //================================================

        void readHPfile()
        {
            try
            {

                if (checkFTG_HP.IsChecked.Value)
                {
                    string path1 = path_root + "/p1HP.txt";
                    string path2 = path_root + "/p2HP.txt";
                    int h1 = TheTool.getInt(TheTool.read_File_get1String(path1));
                    int h2 = TheTool.getInt(TheTool.read_File_get1String(path2));
                    if (checkAutoReset.IsChecked.Value)
                    {
                        if (h1 > hp_p1 ) // 0 > -322 h1 > hp_p1 && firstTime == true   0 > -100
                        {
                           
                            a1.reset();
                            a2.reset();
                            updateGUI();
                            CountToAdjustStrength();
                            groupAndcountUser();
                            Console.WriteLine("text one side = "+ txt_oneSide_avg);
                            countP1 = 0;
                            
                        }
                        
                    }
                    hp_p1 = h1;
                    hp_p2 = h2;
                    //Console.WriteLine(hp_p1 + "_" + hp_p2);
                }
            }
            catch { }
        }

        private void txtLog_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
        //ONE SIDE MODE ===================================================================


        //private void startTime_Click(object sender, RoutedEventArgs e)
        //{
        //    //Timer from create messages for P2
        //    if(!fisrtClick)
        //    {
        //        fisrtClick = true;
        //        myTimer2 = new Timer();
        //        myTimer2.Interval = 60000 / (3 * int.Parse(numParti.Text));
        //        myTimer2.Elapsed += myTimer2_Elapsed;
        //        myTimer2.Start();
        //        Console.WriteLine("start mytimer 2");
        //    }

        //}

        //private void stopTime_Click(object sender, RoutedEventArgs e)
        //{
        //    myTimer2.Stop();
        //    myTimer2.Dispose();
        //    fisrtClick = false;
        //    Console.WriteLine("stop mytimer 2");
        //}

        //private void myTimer2_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    getMsg("AutoMessage", "P2+");
        //}


        private void checkPause_Checked(object sender, RoutedEventArgs e)
        {
            isPause = checkPause.IsChecked.Value;
        }

        private void checkOneSide_Checked(object sender, RoutedEventArgs e)
        {
            oneSideMode_setUp();
        }

        void oneSideMode_setUp()
        {
            if (!initialized) { return;  }
            try
            {
                if (oneSide_Timer != null)
                {
                    oneSide_Timer.Stop();
                    oneSide_Timer.Dispose();
                }
            }
            catch { }
            isOneSideMode = checkOneside.IsChecked.Value;
            //oneSide_generateEvery = int.Parse(txt_oneSide_rate.Text);
            oneSide_generateEvery = TheTool.getInt(txt_oneSide_rate);
            //
            if (isOneSideMode) { oneSideMode_startTimer(); }
        }


        private void oneSideMode_startTimer()
        {
            oneSide_Timer = new Timer();
            oneSide_Timer.Interval = oneSide_generateEvery;
            oneSide_Timer.Elapsed += oneSide_Timer_Elapsed;
            oneSide_Timer.Start();
        }

        private void oneSide_Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (oneSide_nextDirection) { getMsg_autoP2("AutoCheer");  }
            else { getMsg_autoP2("AutoJeer");  }            
            oneSide_nextDirection = !oneSide_nextDirection;

            /*if (oneSide_nextDirection) { getMsg_autoP2("P2+"); }
            else { getMsg_autoP2("P1-"); }
            oneSide_nextDirection = !oneSide_nextDirection;*/
        }

        private void but_oneSide_cal_Click(object sender, RoutedEventArgs e)
        {
            oneSide_numParticipant = TheTool.getInt(txt_oneSide_numParticipant);
            oneSide_avgMsg_perMinParticipant = TheTool.getDouble(txt_oneSide_avg);
            double d = 60000 / (oneSide_avgMsg_perMinParticipant * oneSide_numParticipant);
            oneSide_generateEvery = (int) d;
            txt_oneSide_rate.Text = oneSide_generateEvery.ToString();
            oneSide_Timer.Stop();
            oneSide_Timer.Dispose();
            oneSideMode_startTimer();
        }

        private void CountToAdjustStrength()
        {
            oneSide_numParticipant = TheTool.getInt(txt_oneSide_numParticipant);
            double sum = countP1 / oneSide_numParticipant;
            if(sum >= 3)
            {
                sum = 3;
            }
            if(sum <= 0)
            {
                sum = 1;
            }
            
            Console.WriteLine("sum = " + sum);
            double d = 60000 / (sum * oneSide_numParticipant);
            oneSide_generateEvery = (int)d;
            txt_oneSide_rate.Text = oneSide_generateEvery.ToString();
            txt_oneSide_avg.Text = sum.ToString();
            oneSide_Timer.Stop();
            oneSide_Timer.Dispose();
            oneSideMode_startTimer();


        }

        class TwitchCountData
        {
            public String txt = "";
            public int count = 0;
        }

        /* private void checkAutoReset_Checked(object sender, RoutedEventArgs e)
         {

         }*/


        //ONE SIDE MODE ===================================================================

    }

   
}
