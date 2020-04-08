using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSA_and_session_key
{
    public partial class Form1 : Form
    {
        BigInteger[] basicKey;
        BigInteger[] sessionKey;
        public Form1()
        {
            InitializeComponent();
        }

        private BigInteger[] genKey(int p, int q)
        {
                BigInteger n = p * q;
                BigInteger fn = (p - 1) * (q - 1);
                BigInteger e = primeValue.genPrime((int)(fn/2)); // деление на 2 используем, чтобы не брать слишком большое E
                BigInteger d = modInverse(e, fn);

            return new BigInteger[3] { e, n, d };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int p = Convert.ToInt32(textBox1.Text);
                int q = Convert.ToInt32(textBox2.Text);
                basicKey = genKey(p, q);
                sessionKey = genKey(primeValue.genPrime(new Random().Next(3000, 5000)), primeValue.genPrime(new Random().Next(5000, 8000)));

                textBox3.Text = basicKey[0].ToString();
                textBox4.Text = basicKey[1].ToString();
                textBox5.Text = basicKey[2].ToString();
                textBox6.Text = basicKey[1].ToString();
                               
                textBox7.Text =  encrypt(sessionKey[0].ToString(), (int)basicKey[0], (int)basicKey[1]).ToString();
                textBox17.Text = encrypt(sessionKey[1].ToString(), (int)basicKey[0], (int)basicKey[1]).ToString();
                textBox10.Text = encrypt(sessionKey[2].ToString(), (int)basicKey[0], (int)basicKey[1]).ToString();
                textBox18.Text = encrypt(sessionKey[1].ToString(), (int)basicKey[0], (int)basicKey[1]).ToString();

                string s1 = encrypt(sessionKey[0].ToString(), (int)basicKey[0], (int)basicKey[1]).ToString();
                string s2 = decrypt(s1, (int) basicKey[2], (int)basicKey[1]).ToString();

            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }
        BigInteger modInverse(BigInteger a, BigInteger n)  //инверсия в кольце по модулю, взято и переделано с https://stackoverflow.com/questions/7483706/c-sharp-modinverse-function
        {
            BigInteger i = n, v = 0, d = 1;
            while (a > 0)
            {
                BigInteger t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }
        private BigInteger crypt(BigInteger m,int ed, int n) //либо E, либо D
        {
            return BigInteger.ModPow(m, ed, n);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                BigInteger sessionE = BigInteger.Parse(decrypt(textBox8.Text, (int)basicKey[2], (int)basicKey[1]));
                BigInteger sessionN = BigInteger.Parse(decrypt(textBox11.Text, (int)basicKey[2], (int)basicKey[1]));
                textBox12.Text = encrypt(textBox9.Text, (int)sessionE, (int)sessionN);

            } catch
            {
                MessageBox.Show("Ошибка");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                BigInteger sessionD = BigInteger.Parse(decrypt(textBox13.Text, (int)basicKey[2], (int)basicKey[1]));
                BigInteger sessionN = BigInteger.Parse(decrypt(textBox14.Text, (int)basicKey[2], (int)basicKey[1]));

                textBox16.Text = decrypt(textBox15.Text, (int)sessionD, (int)sessionN);

            }
            catch
            {
                MessageBox.Show("Ошибка");
            }
        }

        private string encrypt(string text,int e,int n)
        {
            string outText = "";
            for (int i = 0; i < text.Length; i++)
            {
                outText += crypt(BigInteger.Parse(((int)text[i]).ToString()), e, n).ToString().PadLeft(11, '0');
            }
            return outText;
        }
        private string decrypt(string text,int d,int n)
        {
            string outText = "";
            for (int i = 0; i < text.Length / 11; i++)
            {
                outText += (char)crypt(BigInteger.Parse(text.Substring(i * 11, 11)), d, n);
            }
            return outText;
        }   
    }
}
