using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace BingoBingo
{
    public partial class BingoBingo : Form
    {
        List<int> listBingo = new List<int>();
        int[] Output = new int[20];
        List<CheckBox> mycheckbox = new List<CheckBox>();
        List<CheckBox> mycheckbox2 = new List<CheckBox>();
        List<CheckBox> mycheckbox3 = new List<CheckBox>();
        int Wincout1 = 0;
        bool Game2Win = false;
        bool Game3Win = false;
        bool isDuplicate = false;
        bool isMatch = false;
        int PriceperCount = 25;
        int TotalReward = 0;
        bool shouldReset = false;

        public BingoBingo()
        {
            InitializeComponent();
        }

        private void BingoBingo_Load(object sender, EventArgs e)
        {
            mycheckbox.AddRange(new CheckBox[] { checkbox1, checkBox2, checkBox3, checkBox4, checkBox5, checkBox6, checkBox7, checkBox8, checkBox9, checkBox10 });
            mycheckbox2.AddRange(new CheckBox[] { checkOdd, checkEven, checkEqual, checkSmallOdd, checkSmallEven  });
            mycheckbox3.AddRange(new CheckBox[] {checkBig,checkSmall });
        }

        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        //public static int RandomNumber(int min, int max)
        //{
        //    lock (syncLock)
        //    { // synchronize
        //        return random.Next(min, max);
        //    }
        //}

        private string Random()
        {
            string temp = "";
            Random rnd = new Random();
            for (int i = 0; i < 20; i++)
            {
                Output[i] = rnd.Next(1, 81);
                for(int j = 0; j<i;j++)
                {
                    while (Output[i] == Output[j])
                    {
                        j = 0;
                        Output[i] = rnd.Next(1,81);
                    }
                }
            }
            Array.Sort(Output);
            for(int i = 0 ; i < Output.Length;i++)
            {
                temp += Output[i];
                if (i < 20 - 1)
                {
                    temp += ", ";
                }
            }
            return temp;
        }

        private void CheckDuplicate(List<int> templist)
        {
            for (int i = 0; i < templist.Count; i++)
            {
                for (int j = 0; j < templist.Count; j++)
                {
                    if (templist[i] == templist[j] && i != j)
                    {
                        MessageBox.Show("有重複的號碼，請重新輸入！");
                        isDuplicate = true;
                        break;
                    }
                }
            }
        }


        CancellationTokenSource source = new CancellationTokenSource();
        private void Game1AddOn ()
        {
            TotalReward += (PriceperCount * listBingo.Count * Wincout1) - PriceperCount * listBingo.Count;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (shouldReset)
            {
                MessageBox.Show("請先重置遊戲！");
                return;
            }
            int boxcount = 0;
            int boxcount2 = 0;
            int boxcount3 = 0;
            for (int i = 0; i < mycheckbox.Count; i++)
            {
                if (mycheckbox[i].Checked == true)
                {
                    boxcount++;
                }
                if (boxcount > 1)
                {
                    MessageBox.Show("只能勾選一項玩法，請重新再試！");
                    break;
                }
            }
            if (boxcount == 0)
            {
                MessageBox.Show("沒有勾選玩法，請重新再試！");
            }
            for(int i = 0; i < mycheckbox2.Count;i++)
            {
                if (mycheckbox2[i].Checked)
                {
                    boxcount2++;
                }
                if(boxcount2>1)
                {
                    MessageBox.Show("猜單雙只能勾選一項，請重新再試！");
                    break;
                }
            }
            if(boxcount2==0)
            {
                MessageBox.Show("沒有勾選猜單雙，請重新再試！");
            }
            for (int i = 0; i < mycheckbox3.Count; i++)
            {
                if (mycheckbox3[i].Checked)
                {
                    boxcount3++;
                }
                if (boxcount3 > 1)
                {
                    MessageBox.Show("比大小只能勾選一項，請重新再試！");
                    break;
                }
            }
            if (boxcount3 == 0)
            {
                MessageBox.Show("沒有勾選比大小，請重新再試！");
            }
            if (boxcount == 1&&boxcount2==1&&boxcount3==1)
            {
                shouldReset = true;
                try
                {
                    List<string> templist = txtMyNum.Text.Split(',', ' ').ToList();
                    listBingo = templist.Select(int.Parse).ToList();
                    for (int m = 0; m < listBingo.Count; m++)
                    {
                        if (listBingo[m] < 1 || listBingo[m] > 80)
                        {
                            throw new NumberNotFoundException();
                        }
                    }
                    CheckDuplicate(listBingo);
                    if (isDuplicate == false)
                    {
                        txtResult.Text = Random();
                        for (int i = 0; i < mycheckbox.Count; i++)
                        {
                            if (listBingo.Count == i + 1)
                            {
                                for (int k = 0; k < listBingo.Count; k++)
                                {
                                    for (int j = 0; j < Output.Length; j++)
                                    {
                                        if (Output[j] == listBingo[i])
                                        {
                                            Wincout1++;
                                        }
                                    }
                                }
                                Game1AddOn();
                                isMatch = true;
                                break;
                            }
                        }
                        if (!isMatch)
                        {
                            MessageBox.Show("數字數量錯誤，請重新輸入");
                        }
                    }
                    if (boxcount2 == 1)
                    {
                        Game2();
                    }
                    if (boxcount3 == 1)
                    {
                        Game3();
                    }
                    MessageBox.Show($"======本輪結算======\n中獎的號碼有{Wincout1}個\n猜單雙有無中獎:{Game2Win}\n比大小有無中獎:{Game3Win}\n@@@@@@總共獲得:{TotalReward}元@@@@@@");
                }
                catch(NumberNotFoundException)
                {
                    MessageBox.Show("數字超過範圍");
                }
                catch
                {
                    MessageBox.Show("數字不可為空");
                }
            }
            
           
           

        }

        private void Game2()
        {
            if (checkOdd.Checked)
            {
                int odd = 0;
                for(int i=0;i<Output.Length;i++)
                {
                    if (Output[i] % 2 != 0)
                    {
                        odd++;
                    }
                }
                if(odd>=13)
                {
                    Game2Win = true;
                }
            }
            if(checkEven.Checked)
            {
                int even = 0;
                for (int i = 0; i < Output.Length; i++)
                {
                    if (Output[i]%2==0)
                    {
                        even++;
                    }
                }
                if (even >= 13)
                {
                    Game2Win = true;
                }
            }
            if (checkEqual.Checked)
            {
                int odd = 0;
                int even = 0;
                for (int i = 0; i < Output.Length; i++)
                {
                    if (Output[i] % 2 == 0)
                    {
                        even++;
                    }
                    if (Output[i] % 2 != 0)
                    {
                        odd++;
                    }
                }
                if (even == 10 && odd == 10)
                {
                    Game2Win = true;
                }
            }
            if(checkSmallOdd.Checked) 
            {
                int odd = 0;
                for (int i = 0; i < Output.Length; i++)
                {
                    if (Output[i] % 2 != 0)
                    {
                        odd++;
                    }
                }
                if (odd == 11||odd==12)
                {
                    Game2Win = true;
                }
            }
            if(checkSmallEven.Checked)
            {
                int even = 0;
                for (int i = 0; i < Output.Length; i++)
                {
                    if (Output[i] % 2 == 0)
                    {
                        even++;
                    }
                }
                if (even == 11||even==12)
                {
                    Game2Win = true;
                }
            }
            if(Game2Win)
            {
                TotalReward += PriceperCount * listBingo.Count;
            }

        }

        private void Game3()
        {
            if(checkBig.Checked)
            {
                int big = 0;
                for(int i = 0; i < Output.Length ; i++)
                {
                    if (Output[i]>40)
                    {
                        big++;
                    }
                }
                if(big >= 13)
                {
                    Game3Win = true;
                }
            }
            if(checkSmall.Checked)
            {
                int small = 0;
                for (int i = 0; i < Output.Length; i++)
                {
                    if (Output[i] < 40)
                    {
                        small++;
                    }
                }
                if (small >= 13)
                {
                    Game3Win = true;
                }
                if(Game3Win)
                {
                    TotalReward += PriceperCount * listBingo.Count;
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtResult.Text = default;
            txtMyNum.Text = default;
            listBingo = new List<int>();
            Output = new int[20];
            Wincout1 = 0;
            Game2Win = false;
            Game3Win = false;
            isDuplicate = false;
            isMatch = false;
            PriceperCount = 25;
            TotalReward = 0;
            shouldReset = false;
            for (int i = 0; i < mycheckbox.Count; i++)
            {
                mycheckbox[i].Checked = default;
            }
            for (int i= 0; i<mycheckbox2.Count; i++)
            {
                mycheckbox2[i].Checked = default;
            }
            for(int i = 0; i<mycheckbox3.Count; i++)
            {
                mycheckbox3[i].Checked = default;
            }
        }

        private void btnExplain_Click(object sender, EventArgs e)
        {
            MessageBox.Show("=========歡迎來到Bingo Bingo=========");
            MessageBox.Show("「BINGO BINGO賓果賓果」是一種每五分鐘開獎一次的電腦彩券遊戲。\n選號範圍為01~80，您可以任意選擇玩1~10個號碼的玩法，\n每次開獎時，電腦系統將隨機開出20個獎號，您可以依您選擇的玩法和選號進行對獎。\n如您的選號符合該期任一種中獎情形，即為中獎，並可依規定兌領獎金。");
            MessageBox.Show("「猜大小」為BINGO BINGO賓果賓果的附加玩法，您可就當期BINGO BINGO賓果賓果可能開出之獎號進行預測，\n認為較小的號碼（01~40號）開出的數目將等於或多於13個號碼者，選擇投注「猜小」；\n認為較大的號碼（41~80號）開出的數目將等於或多於13個號碼者，選擇投注「猜大」，只要您猜中即為中獎。");
            MessageBox.Show("「猜單雙」玩法共有五種開獎結果供您進行預測：\n認為單數號碼開出的數目將等於或多於13個號碼者，選擇投注「單」；\n認為雙數號碼開出的數目將等於或多於13個號碼者，選擇投注「雙」；\n認為單數號碼開出的數目將等於11或12個號碼者，選擇投注「小單」；\n認為雙數號碼開出的數目將等於11或12個號碼者，選擇投注「小雙」；\n認為單數與雙數號碼各開出10個號碼者，選擇投注「和」。\n如您猜中當期的開獎結果，即為中獎，即可依猜單雙獎金表兌領獎金。");

        }

        private void btnPrice_Click(object sender, EventArgs e)
        {
            MessageBox.Show("=========遊戲售價=========");
            MessageBox.Show("@@@@@BINGO BINGO賓果賓果@@@@@\n1.每注售價為新臺幣25元(每注之選號數須與所選玩法一致。例如，「1星」玩法每注應選1個號碼；「4星」玩法每注應選4個號碼，以此類推)。\n2.如投注多期，則總投注金額為各組之單組投注金額乘以投注期數之總和。");
            MessageBox.Show("@@@@@猜大小@@@@@\n1.每注售價為新臺幣25元。\n2.如「BINGO BINGO賓果賓果」選擇投注多期時，則猜大小之總投注金額為投注金額乘以投注期數之總和。");
            MessageBox.Show("@@@@@猜單雙@@@@@\n1.每注售價為新臺幣25元。\n2.如「BINGO BINGO賓果賓果」選擇投注多期時，則猜大小之總投注金額為投注金額乘以投注期數之總和。");
        }
    }
}
public class NumberNotFoundException : Exception
{
    public NumberNotFoundException()
    {
    }

    public NumberNotFoundException(string message)
        : base(message)
    {
    }

    public NumberNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
