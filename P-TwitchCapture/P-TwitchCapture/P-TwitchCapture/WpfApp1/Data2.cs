using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTwitchCapture
{
    class Data2
    {
        public int i = 0;
        public String time = "";
        public int[] score_pos = { 0, 0, 0 };//positive
        public int[] score_neg = { 0, 0, 0 };
        public double dominance = 0;//[-1,1]
        public double f_p1 = 0;
        public double f_p2 = 0;
    }
}
