using SentimentAnalysisConsoleApp.DataStructures;
using System;
using System.Windows;
using System.Windows.Media;
using System.Timers;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace PTwitchCapture
{

    public partial class MainWindow : Window
    {
        int countP1 = 0;
        int countP2 = 0;
        public MainWindow()
        {
            countP1 = 0;
            countP2 = 0;
            TheTool.Sys = this;
            InitializeComponent();
            bot = new Bot(this);
            applySetting();
            initThread();

            this.Closing += new System.ComponentModel.CancelEventHandler(Window4_Closing);
        }
        
        Bot bot;
        Analysis1 a1 = new Analysis1();
        Analysis2 a2 = new Analysis2();
        

        Boolean isPause = false;

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



        void processV2_part1(string msg)
        {
            a2.addMsg(msg);
            countExport();
        }

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
        public void showError(string s, Boolean b) { Console.WriteLine("EX " + s); }
        public void showError(string s) { Console.WriteLine("EX " + s); }
        public void showError(Exception ex) { Console.WriteLine("EX " + ex.ToString()); }

        void countExport()
        {
                string path3 = path_root + "/countP1.txt";
                countP1++;
                string a = countP1.ToString();
                TheTool.writeFile(path3, a, true);
           
        }

        void exportFTG()
        {
            //if (checkFTG.IsChecked.Value)
            //{
            //    string path1 = path_root + "/N.txt";
            //    string a = Math.Round(a1.f_p1, 2) + Environment.NewLine + "0";
            //    TheTool.writeFile(path1, a, true);
            //}
            //if (checkFTG2.IsChecked.Value)
            //{
            //    string path2 = path_root + "/P.txt";
            //    string b = Math.Round(a1.f_p2, 2) + Environment.NewLine + "0";
            //    TheTool.writeFile(path2, b, true);
            //}
            //Console.WriteLine("P1=" + a1.f_p1 + " P2=" + a1.f_p2);

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

        Timer myTimer;
        static volatile bool isRunning;

        public void initThread()
        {
            myTimer = new Timer();
            myTimer.Interval = 1000;
            myTimer.Elapsed += myTimer_Elapsed;
            myTimer.Start();
        }

        private void myTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isRunning) return;
            isRunning = true;
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    processV2_part2();
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

        private void checkPause_Checked(object sender, RoutedEventArgs e)
        {
            isPause = checkPause.IsChecked.Value;
        }

        //==========================================================================================
        Boolean dataCollect_on = false;
        Boolean dataCollect_autostart = true;
        int i_d = 0;//index of data
        List<Data2> list_data = new List<Data2>();
        List<Data_Message> list_data_msg = new List<Data_Message>();

        //public static string path_saveFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\file";
        public static string path_saveFolder = "file";



        void clearData()
        {
            i_d = 0;
            list_data.Clear();
            list_data_msg.Clear();
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

        List<String> convertData2()
        {
            List<string> list = new List<String>();
            list.Add("id,time,dominance,pos,pos1,pos2,neg,neg1,neg2,f_p1,f_p2");
            foreach (Data2 d in list_data)
            {
                String t = d.i + "," + d.time + "," + d.dominance + "," +
                    d.score_pos[0] + "," + d.score_pos[1] + "," + d.score_pos[2] + "," +
                    d.score_neg[0] + "," + d.score_neg[1] + "," + d.score_neg[2] + "," +
                    d.f_p1 + "," + d.f_p2;
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
        }

        private void checkFTG_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtFile_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
