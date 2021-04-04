using SentimentAnalysisConsoleApp.DataStructures;
using System;

namespace PTwitchCapture
{
    class Analysis1
    {
        //Index: 0 Total, 1 P1, P2
        public int[] score_pos = { 0, 0, 0 };//positive
        public int[] score_neg = { 0, 0, 0 };

        //----------------
        //public int count_p1 = 0;
        //public int count_p2 = 0;
        //public int count_total = 0;
        //
        public double bal = 0;//[-1,1]
        public int bal_gui = 0;//[-100,100]
        public int bal_hp;
        //
        public double f_p1 = 0;
        public double f_p2 = 0;
        public double f_p1_gui = 0;
        public double f_p2_gui = 0;

        public void reset()
        {
            score_pos = new int[] { 0, 0, 0 };
            score_neg = new int[] { 0, 0, 0 };
            //count_p1 = 0;
            //count_p2 = 0;
            //count_total = 0;
            bal = 0;
            bal_gui = 0;
            bal_hp = 0;
            f_p1 = 0;
            f_p2 = 0;
            f_p1_gui = 0;
            f_p2_gui = 0;
    }

        public String analyzeMsg(string m, Boolean sentimentAnalysis)
        {
            if (sentimentAnalysis) { return analyzeMsg_withSentiment(m); }
            //
            String result = "";
            if (m.Contains("P1+")) { 
                score_pos[0]++;
                score_pos[1]++;
            }
            if (m.Contains("P2+"))
            {
                score_pos[0]++;
                score_pos[2]++;
            }
            if (m.Contains("P1-"))
            {
                score_neg[0]++;
                score_neg[1]++;
            }
            if (m.Contains("P2-"))
            {
                score_neg[0]++;
                score_neg[2]++;
            }
            updateVariables();
            return result;
        }

        public void updateVariables()
        {
            double d1 = score_pos[1] - score_neg[1] - score_pos[2] + score_neg[2];
            double d2 = score_pos[0] + score_neg[0];
            bal = d1 / d2;
            bal_gui = (int)Math.Round(bal * 100);
            bal_hp = (int)Math.Round(bal * 120);
            f_p1 = -bal;
            f_p2 = bal;
            f_p1_gui = bal_gui;
            f_p2_gui = -bal_gui;
        }

        public String analyzeMsg_withSentiment(string m)
        {
            String result = "";
            int target_player = 0;
            if (m.Contains("P1") && !m.Contains("P2")) { target_player = 1; }
            else if (m.Contains("P2") && !m.Contains("P1")) { target_player = 2; }
            if (target_player != 0)
            {
                    SentimentPrediction resultprediction = SentimentAnalyzer.predict(m);
                    Boolean neg = Convert.ToBoolean(resultprediction.Prediction);
                    if (neg)
                    {
                        score_neg[0]++;
                        score_neg[target_player]++;
                    }
                    else
                    {
                        score_pos[0]++;
                        score_pos[target_player]++;
                    }
                updateVariables();
                    result = $"  P{target_player} {(neg ? "-" : "+")} c={resultprediction.Probability}";

            }
            return result;
        }

        public string getMsgVar()
        {
            String s = "Balance: " + Math.Round(bal,2) + Environment.NewLine;
            if (bal_hp >= 0)
            {
                s += "P1 wins by ΔHP " + bal_hp + Environment.NewLine;
            }
            else
            {
                s += "P2 wins by ΔHP " + -bal_hp + Environment.NewLine;
            }
            s += Environment.NewLine;
            //s += "Score All: " + count_total + Environment.NewLine;
            //s += "Score P1: " + count_p1 + Environment.NewLine;
            //s += "Score P2: " + count_p2 + Environment.NewLine;
            s += "Score All: +" + score_pos[0] + " -" + score_neg[0] + Environment.NewLine;
            s += "Score P1: +" + score_pos[1] + " -" + score_neg[1] + Environment.NewLine;
            s += "Score P2: +" + score_pos[2] + " -" + score_neg[2] + Environment.NewLine;
            s += Environment.NewLine;
            s += "P1's F: " + Math.Round(f_p1, 2) + Environment.NewLine;
            s += "P2's F: " + Math.Round(f_p2, 2) + Environment.NewLine;
            return s;
        }

    }
}
