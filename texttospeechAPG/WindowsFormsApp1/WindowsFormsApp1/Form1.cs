using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.Threading;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string textForSpeak, textP1, textP2, textCheering, P1,P2, textAll;
        public string checkNewAudio = "A";
        public string oldAudio = "A", commentP1strong = "Now, P1 is stronger than P2." , commentP2strong = "Now, P2 is stronger than P1.";
        string copyWords;
        string midiAll;
        string[] words, midi;
        bool rap = true;
        int cSpeech = 0, typeMidi = 0, typeMusic = 0, newStrong=0;
        double cMidi = 0, cMidiFuture = 0;

        public int typeRhythm = 0;
        public bool checkBGM,p1Strong,p2Strong;
        public int setTime = -999;
        public double beat = 750;
        public double P1Value = 1, P2Value = 0;
        public int setTimeP = 2000;
        string path1 = @"C:\Users\maili\Desktop\texttospeechAPG\textfile\"; //real comment script
        string path2 = @"D:\FTGexp\F\"; //All state feom FightingICE
        public Form1()
        {
            InitializeComponent();
        }

        private void readMidi()
        {
            System.IO.File.WriteAllText(path1+"testComments.txt", "");
            typeMidi = 0;
            Random random = new Random();
            int tyrandomNumber = random.Next(0, 3);
            midi = null;
            typeMidi = tyrandomNumber;
            if (typeMidi == 0)
            {
                midiAll = System.IO.File.ReadAllText(path1+"How Many Mics testing(MIDI).txt");
            }
            else if (typeMidi == 1)
            {
                midiAll = System.IO.File.ReadAllText(path1+"Dove.txt");
            }
            else if (typeMidi == 2)
            {
                midiAll = System.IO.File.ReadAllText(path1+"Pras.txt");
            }
            else
            {

                midiAll = System.IO.File.ReadAllText(path1+"Lost Yourself(MIDI).txt");
            }
            for (int i = 0; i < midiAll.Length; i++)
            {
                midi = midiAll.Split(',', '\n', ' ');

            }
            //Console.WriteLine(midi.Length + " " + typeMidi);
            //haveMidi();
        }
        private void rePlay()
        {
            //backingBeat();
        }
        private void backingBeat()
        {
            /* System.Media.SoundPlayer soundP = new System.Media.SoundPlayer();
             Random random = new Random();
             int tyrandomMusic = random.Next(0, 2);
             typeMusic = tyrandomMusic;
             //typeMusic = 1;
             if (typeMusic == 0)
             {
                 soundP.SoundLocation = @"C:\Users\Thanat Jumneanbun\Documents\TextToSpeech1\backingBeat\FreeBeats.wav";

             }
             else if (typeMusic == 1)
             {
                 soundP.SoundLocation = @"C:\Users\Thanat Jumneanbun\Documents\TextToSpeech1\backingBeat\FreeBeats2.wav";

             }*/


            chooseBeat();
            //soundP.Load();
            //soundP.Play();

            //Thread.Sleep(setTimeP);
            //readWords();

        }

        private void chooseBeat()
        {
            if (typeMusic == 0)
            {
                if (typeMidi == 0)
                {
                    beat = 900;
                    setTimeP = 7000;
                }
                else if (typeMidi == 1)
                {
                    beat = 900;
                    setTimeP = 7500;
                }
                else if (typeMidi == 2)
                {
                    beat = 900;
                    setTimeP = 7000;
                }
                else
                {
                    beat = 900;
                    setTimeP = 7800;
                }
            }
            else if (typeMusic == 1)
            {
                if (typeMidi == 0)
                {
                    beat = 900;
                    setTimeP = 9000;
                }
                else if (typeMidi == 1)
                {
                    beat = 900;
                    setTimeP = 9000;
                }
                else if (typeMidi == 2)
                {
                    beat = 900;
                    setTimeP = 9800;
                }
                else
                {
                    beat = 900;
                    setTimeP = 9000;
                }
            }
        }

        private void readWords()
        {
            
            try
            {
                DateTime fileCreatedDate1 = File.GetLastWriteTime(path2+"countP1.txt");
                checkNewAudio = fileCreatedDate1.ToString("HH:mm:ss");
                //textForSpeak = System.IO.File.ReadAllText(@"C:\Users\Thanat Jumneanbun\Documents\texttospeechAPG\textfile\testComments.txt");
                textP1 = System.IO.File.ReadAllText(path1+"P1Comment.txt");
                textP2 = System.IO.File.ReadAllText(path1+"P2Comment.txt");
                textCheering = System.IO.File.ReadAllText(path1+"Cheering.txt");
                P1 = System.IO.File.ReadLines(path2+"P1Hp.txt").First();
                P2 = System.IO.File.ReadLines(path2+ "P2Hp.txt").First();
                Double.TryParse(P1, out P1Value);
                Double.TryParse(P2, out P2Value);
            }
            catch
            {
                DateTime fileCreatedDate1 = File.GetLastWriteTime(path2+"countP1.txt");
                checkNewAudio = fileCreatedDate1.ToString("HH:mm:ss");
                textP1 = System.IO.File.ReadAllText(path1+"P1Comment.txt");
                textP2 = System.IO.File.ReadAllText(path1+"P2Comment.txt");
                textCheering = System.IO.File.ReadAllText(path1+"Cheering.txt");
                P1 = System.IO.File.ReadLines(path2 + "P1Hp.txt").First();
                P2 = System.IO.File.ReadLines(path2 + "P2Hp.txt").First();
                Double.TryParse(P1, out P1Value);
                Double.TryParse(P2, out P2Value);
            }

            if (oldAudio == checkNewAudio)
            {

                if (P1Value > P2Value)
                {
                    if (p1Strong == false)
                    {
                        p1Strong = true;
                        p2Strong = false;
                        textAll = textP1;
                        textForSpeak = textAll;
                        textForSpeak = commentP1strong;
                        System.IO.File.WriteAllText(path1+"testComments.txt", textForSpeak);

                    }
                    else
                    {
                        textAll = textP1;
                        textForSpeak = textAll;
                        System.IO.File.WriteAllText(path1+"testComments.txt", commentP1strong);
                    }
                }
                else if(P1Value == P2Value)
                {
                    System.IO.File.WriteAllText(path1+"testComments.txt", " ");
                    Random random = new Random();
                    int tyrandomNumber = random.Next(0, 2);
                    if(tyrandomNumber ==0)
                    {
                        textAll = textP1;
                        textForSpeak = textAll;
                    }
                    else
                    {
                        textAll = textP2;
                        textForSpeak = textAll;
                    }
                    


                }
                else if (P1Value < P2Value)
                {
                    
                    if (p2Strong == false)
                    {
                        p1Strong = false;
                        p2Strong = true;
                        textAll = textP2;
                        textForSpeak = textAll;
                        textForSpeak = commentP2strong;
                        System.IO.File.WriteAllText(path1+"testComments.txt", textForSpeak);
                    }
                    else
                    {
                        textAll = textP2;
                        textForSpeak = textAll;
                        System.IO.File.WriteAllText(path1+"testComments.txt", commentP2strong);
                    }

                }
                oldAudio = checkNewAudio;
            }
            else
            {
                textAll = textCheering;
                textForSpeak = textAll;
                oldAudio = checkNewAudio;
            }
            /*Console.WriteLine("check text all: " + textForSpeak);
            Console.WriteLine("check text all: " + textForSpeak.Length);
            Console.WriteLine("check P1 text all: " + textP1);
            Console.WriteLine("check P2 text all: " + textP2);*/
            Console.WriteLine("check P1 Value: " + P1Value);
            Console.WriteLine("check P2 Value: " + P2Value);

             if(rap)
            {
                if (textAll != copyWords)
                {
                    System.IO.File.WriteAllText(path1+"subComments.txt", textForSpeak);
                    for (int i = 0; i < textForSpeak.Length; i++)
                    {
                        words = textForSpeak.Split(' ', '\n');

                    }

                    //Console.WriteLine("words= " + words);
                    //Console.WriteLine("textForSpeak.Length= " + textForSpeak.Length);
                    //speechWord();
                }
                else
                {
                    introBeforeCommnet();
                }
            }
            

        }


        private void speechWord()
        {
            //rhythm();
            for (int i = 0; i < midi.Length; i++)
            {
                double.TryParse(midi[i], out cMidi);
                if (i < midi.Length - 1)
                {
                    double.TryParse(midi[i + 1], out cMidiFuture);
                }

                if (cMidiFuture < 0)
                {
                    cMidi = cMidi + (cMidiFuture * -1);
                    i++;
                }
                //Console.WriteLine("I= "+i+" cMidi= "+cMidi);

                setTime = Convert.ToInt32(beat * cMidi);
                Console.WriteLine(setTime+" = "+beat+" * "+ cMidi);
                addSpeed();

            }
            //after read all rythm done
            cSpeech = 0;

            // introBeforeCommnet();
            //Speech.synth.Dispose();
            readMidi();
            readWords();
            speechWord();

        }

        private void addSpeed()
        {
            if (setTime >= (beat * 1)) //1.0
            {
                Speech.rateSpeed = 1;
            }
            else if (setTime >= (beat * 0.75)) //0.75
            {
                Speech.rateSpeed = 2;
            }
            else if (setTime >= (beat * 0.5)) //0.50
            {
                Speech.rateSpeed = 3;
            }
            else if (setTime >= (beat * 0.33)) //0.33
            {
                Speech.rateSpeed = 4;
            }
            else if (setTime >= (beat * 0.25))
            {
                Speech.rateSpeed = 5;
            }
            else
            {
                Speech.rateSpeed = 6;
            }
            checkWord();
        }

        private void checkWord()
        {
            if (textForSpeak.Length == 0)
            {
                introBeforeCommnet();
            }
            else
            {
                if (cSpeech != words.Length)
                {

                    //Console.WriteLine(cSpeech);

                    //Speech.setLanguage2();
                    //Speech.synth.Dispose();
                    /*if (words[cSpeech].Length <= 5)
                    {
                        Speech.synth.Dispose();
                    }
                    else
                    {
                        setTime = 900;
                    }*/
                    
                    /*if (words[cSpeech].Length < 5 )
                    {
                        //Speech.synth.Dispose();
                        Console.WriteLine("Disposed: " + cSpeech);
                    }*/
                    if (words[cSpeech].Length >= 5)
                    {
                        setTime = 900;
                    }

                    // Console.WriteLine(words[cSpeech]);
                    // Console.WriteLine("words length: "+ words[cSpeech].Length);
                    Speech.speak_cont(words[cSpeech]);
                    // Console.WriteLine("Settime: "+setTime);
                    Thread.Sleep(setTime);
                    
                   

                    cSpeech++;

                }
                else
                {
                    
                    copyWords = textAll;
                    cSpeech = 0;
                    textForSpeak = null;
                    words = null;
                    //Speech.synth.Dispose();
                    //Speech.setLanguage(Speech.getVoiceList()[1]);
                    readWords();
                }

            }

        }

        private void introBeforeCommnet()
        {
            textForSpeak = "yo";
            for (int i = 0; i < textForSpeak.Length; i++)
            {
                words = textForSpeak.Split(' ', '\n');
            }

            //rhythm();
            for (int i = 0; i < words.Length; i++)
            {
                cSpeech = 0;
                System.IO.File.WriteAllText(path1+"subComments.txt", "yo");
                Random random = new Random();
                int preCheck = random.Next(0, 3);
                if (preCheck == 0)
                {
                    cMidi = 0.25;
                }
                else if (preCheck == 1)
                {
                    cMidi = 0.5;
                }
                else
                {
                    cMidi = 0.75;
                }
                //Console.WriteLine("I= " + i + " cMidi= " + cMidi);
                setTime = Convert.ToInt32(beat * cMidi);
                addSpeed();

            }
            //after read all rythm done
            cSpeech = 0;
            Thread.Sleep(700);
        }

        private void normalTTS()
        {
            rap = false;
            readWords();
            //textForSpeak = System.IO.File.ReadAllText(path1+"P1Comment.txt");

            Speech.rateSpeed = 0;
            Speech.speak(textForSpeak);
            copyWords = textForSpeak;
            normalTTS();



        }
        private void rapTTS()
        {
            rap = true;
            System.IO.File.WriteAllText(path1+"subComments.txt", " ");
            System.IO.File.WriteAllText(path1+"testComments.txt", " ");
            System.IO.File.WriteAllText(path1+"P1Comment.txt", " ");
            System.IO.File.WriteAllText(path1+"P2Comment.txt", " ");
            DateTime fileCreatedDate1 = File.GetLastWriteTime(path2+"countP1.txt");
            checkNewAudio = fileCreatedDate1.ToString("HH:mm:ss");
            oldAudio = checkNewAudio;
            readMidi();
            //backingBeat();
            Thread.Sleep(setTimeP);
            introBeforeCommnet();
            readWords();
            Console.WriteLine("readword in button");
            speechWord();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Speech.setLanguage(Speech.getVoiceList()[1]);
            rapTTS();
         // normalTTS();
        }

        public void Exit()
        {
            System.IO.File.WriteAllText(path1+"subComments.txt", " ");
            System.IO.File.WriteAllText(path1+"testComments.txt", " ");


        }
    }
}
