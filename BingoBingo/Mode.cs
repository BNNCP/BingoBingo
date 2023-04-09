using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace BingoBingo
{
    public static class Gamemode
    {
        static int[] mode = { 2 };
        static int[] mode2 = { 1,3 };
        static int[] mode3 = { 1,2,20 };
        static int[] mode4 = { 1, 1, 4, 40 };
        static int[] mode5 = { 1, 1, 2, 20, 300 };
        static int[] mode6 = { 1, 1, 1, 8, 40, 1000 };
        static int[] mode7 = { 1, 1, 1, 2, 12, 120, 3200 };
        static int[] mode8 = { 1, 1, 1, 1, 1, 8, 40, 800, 20000 };
        static int[] mode9 = { 1, 1, 1, 1, 1, 4, 20, 120, 4000, 40000 };
        static int[] mode10 = { 1, 1, 1, 1, 1, 1, 10, 100, 1000, 10000, 200000 };
        public static List<int[]> modeall = new List<int[]>() { mode, mode2, mode3, mode4, mode5, mode6, mode7, mode8, mode9, mode10 };
    }
}
