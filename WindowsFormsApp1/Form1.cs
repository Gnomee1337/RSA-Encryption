using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Security.Cryptography;
using System.IO;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<string> encryptMessage = new List<string>();
        List<string> decryptMessage = new List<string>();
        //For Text
        string message = "";

        //Variables for RSA
        BigInteger p = new BigInteger(), q = new BigInteger(), n = new BigInteger(), fn = new BigInteger(), e = new BigInteger(), d = new BigInteger();

        //CreateFile
        string randomNewDirectory = "TXTOutput" + DateTime.Now.ToString("dd.MM.yyyyInhh-mm-ss");
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

        RandomNumberGenerator rng = RandomNumberGenerator.Create();

        PrimeNumber primeNumber = new PrimeNumber();
        EuclideanAlgorithm euclAlg = new EuclideanAlgorithm();

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            CreateTxt();
        }

        //----------------------------------------------------------------------------\\
        //--------------------------------TXT Output----------------------------------\\
        //----------------------------------------------------------------------------\\

        private void CreateTxt()
        {
            Directory.CreateDirectory(@currentDirectory + @randomNewDirectory);
            using (var myFile = File.Create(@currentDirectory + @randomNewDirectory + "\\TXToutput.txt")) { }
        }

        private void FileOutputEncrypt(string _encryptName)
        {
            using (StreamWriter file = new StreamWriter(@currentDirectory + @randomNewDirectory + "\\TXToutput.txt", true))
            {
                file.WriteLine(_encryptName + Environment.NewLine + "Исходное сообщение:" + message + Environment.NewLine + "Зашифрованное сообщение:");
                    foreach(String s in encryptMessage)
                {
                    file.WriteLine(s);
                }
                file.Close();
            }
        }

        private void FileOutputDecrypt()
        {
            using (StreamWriter file = new StreamWriter(@currentDirectory + @randomNewDirectory + "\\TXToutput.txt", true))
            {
                file.Write("Расшифрованное сообщение:");
                foreach (String s in decryptMessage)
                {
                    file.Write(s);
                }
                file.WriteLine();
                file.Close();
            }
        }

        private void GenerateNumbers()
        {
            p = primeNumber.GeneratePrimeNumber((int)numericUpDown1.Value, (int)numericUpDown2.Value);
            q = primeNumber.GeneratePrimeNumber((int)numericUpDown1.Value, (int)numericUpDown2.Value);
            n = p * q;
            fn = (p - 1) * (q - 1);
            e = e_Generator();
            d = d_Generator();

            //p_textBox.Text = p.ToString();
            //q_textBox.Text = q.ToString();
            //np_textBox.Text = n.ToString();
            //ns_textBox.Text = n.ToString();
            //fn_textBox.Text = fn.ToString();
            //e_textBox.Text = e.ToString();
            //d_textBox.Text = d.ToString();
        }
        private BigInteger e_Generator()
        {
            BigInteger temp = 0;
            for (;;)
            {
                temp = primeNumber.RandomInRange(rng, 1, fn);
                BigInteger NOD = new BigInteger();
                NOD = euclAlg.ClassicEuclidean(temp, fn);
                if (NOD == 1) break;
            }
            return temp;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reminder:"+
                "\n2 ^ 1 = 2bit\n2 ^ 2 = 4bit\n2 ^ 3 = 8bit\n2 ^ 4 = 16bit\n2 ^ 5 = 32bit\n2 ^ 6 = 64bit"+
                "\n2 ^ 7 = 128bit\n2 ^ 8 = 256bit\n2 ^ 9 = 512bit\n2 ^ 10 = 1024bit"+
                "\n2 ^ 11 = 2048bit\n2 ^ 12 = 4096bit\n2 ^ 13 = 8192bit");
        }

        private BigInteger d_Generator()
        {
            BigInteger tempd = 0;
            for (;;)
            {
                BigInteger x = new BigInteger(), y = new BigInteger();
                BigInteger extEucl = new BigInteger();
                extEucl = euclAlg.ExtendEuclidean(fn, e, out x, out y);
                tempd = (x > y) ? x : y;
                BigInteger templ = new BigInteger();
                templ = BigInteger.Multiply(e, tempd);
                BigInteger remainder =  new BigInteger();
                BigInteger GCD = new BigInteger();
                GCD = BigInteger.DivRem(templ, fn, out remainder);
                if (remainder == 1) { break; }
            }
            return tempd;
        }

        public void Encryption()
        {
            message = textBox1.Text;
            BigInteger unicodeNumberChar = new BigInteger(), unicodeTemp = new BigInteger();
            GenerateNumbers();
            for (int i = 0; i < message.Length; i++)
            {
                unicodeNumberChar = (char)message[i];
                unicodeTemp = BigInteger.ModPow(unicodeNumberChar, e, n);
                encryptMessage.Add(unicodeTemp.ToString());
            }
        }

        public void Decryption()
        {
            BigInteger unicodeNumberChar = new BigInteger(), unicodeTemp = new BigInteger();
            for (int i = 0; i < encryptMessage.Count; i++)
            {
                unicodeNumberChar = BigInteger.Parse(encryptMessage[i]);
                unicodeTemp = BigInteger.ModPow(unicodeNumberChar, d, n);
                decryptMessage.Add(((char)unicodeTemp).ToString());
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Encryption();
            //textBox6.Text = string.Join(Environment.NewLine, encryptMessage);
            FileOutputEncrypt("RSA");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Decryption();
            //textBox2.Text = string.Concat(decryptMessage);
            FileOutputDecrypt();
            GC.Collect();
        }

    }
}
