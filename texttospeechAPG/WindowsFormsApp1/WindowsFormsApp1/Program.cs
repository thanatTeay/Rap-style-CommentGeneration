using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;

namespace WindowsFormsApp1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    class Speech
    {
        public static int vol = 100;
        public static int rateSpeed = 0;
        public static SpeechSynthesizer synth = new SpeechSynthesizer { Volume = vol, Rate = rateSpeed };
        public static int voiceGender = 1;
        static Boolean isPause = false;

        static List<SpeechSynthesizer> list_sync = new List<SpeechSynthesizer>();


        public static void initialize()
        {
            SpeechSynthesizer s = new SpeechSynthesizer { Volume = vol, Rate = rateSpeed };

            list_sync.Add(s);
            if (list_sync.Count > 5) { list_sync.First().Dispose(); list_sync.RemoveAt(0); }

            synth = s;


            // synth = new SpeechSynthesizer { Volume = vol, Rate = rateSpeed };
            //setVoice(voiceGender);
            try
            {
                if (lang != "")
                {

                    synth.SelectVoice(lang);
                    // Console.WriteLine("words= " + lang);
                }
            }
            catch
            {
                //initialize();
            }
            isPause = false;

        }


        //public static int i_s = 0;
        //public static SpeechSynthesizer[] sync_arr = new SpeechSynthesizer[] {
        //    new SpeechSynthesizer { Volume = vol, Rate = rateSpeed }, new SpeechSynthesizer { Volume = vol, Rate = rateSpeed }, new SpeechSynthesizer { Volume = vol, Rate = rateSpeed } };


        //public static void initialize()
        //{

        //    sync_arr[i_s].Dispose();
        //    sync_arr[i_s] = new SpeechSynthesizer { Volume = vol, Rate = rateSpeed };


        //    i_s++; if (i_s > 2) { i_s = 0; }
        //    synth = sync_arr[i_s];


        //    synth = new SpeechSynthesizer { Volume = vol, Rate = rateSpeed };




        //    synth = new SpeechSynthesizer { Volume = vol, Rate = rateSpeed };
        //    setVoice(voiceGender);
        //    try
        //    {
        //        if (lang != "")
        //        {

        //            synth.SelectVoice(lang);
        //            Console.WriteLine("words= " + lang);
        //        }
        //    }
        //    catch
        //    {
        //        initialize();
        //    }



        //    isPause = false;


        //}



        public static void exportFile()
        {
            //synth.SetOutputToWaveFile(@"C:\Users\Thanat Jumneanbun\Desktop\TextToSpeech\Wavfile\Sample.wav");

        }

        public static void speak(string s)
        {
            initialize();
            synth.Speak(s);
        }

        public static void speak_cont(string s)
        {
            initialize();
            //synth.Rate = rateSpeed;
            //setVoice(voiceGender);
            synth.SpeakAsync(s);
            

        }


        public static void stop()
        {
            synth.SpeakAsyncCancelAll();
        }

        public static void pause_resume()
        {
            if (isPause) { synth.Resume(); }
            else { synth.Pause(); }
            isPause = !isPause;
        }

        public static void setVol(int i)
        {
            vol = i;//cant change during play
        }

        public static void setVoice(int i)
        {
            if (i == 1) { synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen); }
            else if (i == 0) { synth.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult); }
            else { synth.SelectVoiceByHints(VoiceGender.Neutral); }
        }


        //=========================================================================
        public static string err_log = "";

        //https://msdn.microsoft.com/en-us/library/ms586869(v=vs.110).aspx
        public static List<string> getVoiceList()
        {
            List<string> list_s = new List<string>();
            try
            {
                using (SpeechSynthesizer synth = new SpeechSynthesizer())
                {
                    foreach (InstalledVoice voice in synth.GetInstalledVoices())
                    {
                        VoiceInfo info = voice.VoiceInfo;
                        string AudioFormats = "";
                        foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                        {
                            AudioFormats += String.Format("{0}\n",
                            fmt.EncodingFormat.ToString());
                        }
                        list_s.Add(info.Name);
                    }
                }
            }
            catch (Exception ex) { err_log = ex.ToString(); }
            return list_s;
        }

        public static List<string> getVoiceDetails()
        {
            List<string> list_s = new List<string>();
            try
            {
                using (SpeechSynthesizer synth = new SpeechSynthesizer())
                {
                    // Output information about all of the installed voices. 
                    list_s.Add("Installed voices -");
                    foreach (InstalledVoice voice in synth.GetInstalledVoices())
                    {
                        VoiceInfo info = voice.VoiceInfo;
                        string AudioFormats = "";
                        foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                        {
                            AudioFormats += String.Format("{0}\n",
                            fmt.EncodingFormat.ToString());
                        }
                        list_s.Add(" Name:          " + info.Name);
                        list_s.Add(" Culture:       " + info.Culture);
                        list_s.Add(" Age:           " + info.Age);
                        list_s.Add(" Gender:        " + info.Gender);
                        list_s.Add(" Description:   " + info.Description);
                        list_s.Add(" ID:            " + info.Id);
                        list_s.Add(" Enabled:       " + voice.Enabled);
                        if (info.SupportedAudioFormats.Count != 0)
                        {
                            list_s.Add(" Audio formats: " + AudioFormats);
                        }
                        else
                        {
                            list_s.Add(" No supported audio formats found");
                        }

                        string AdditionalInfo = "";
                        foreach (string key in info.AdditionalInfo.Keys)
                        {
                            AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
                        }

                        list_s.Add(" Additional Info - " + AdditionalInfo);
                        list_s.Add("");
                    }
                }
            }
            catch { }
            return list_s;
        }

        public static string lang = "";
        public static void setLanguage(string l)
        {
            lang = l;
            synth.SelectVoice(lang);
        }

        public static void setLanguage2()
        {
            var syn = new System.Speech.Synthesis.SpeechSynthesizer();
            syn.SelectVoice("Microsoft Server Speech Text to Speech Voice (ja-JP, Haruka)");
            syn.Speak("こんにちは");
        }

    }
}
