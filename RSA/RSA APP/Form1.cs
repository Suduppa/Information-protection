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
using System.Drawing.Printing;



namespace RSA_APP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


           private static BigInteger p = GenerateLargePrime(12);
           private static BigInteger q = GenerateLargePrime(12);  
           private static BigInteger n = p * q;
           private static BigInteger phi = (p - 1) * (q - 1);
           private static BigInteger exp = 65537;
           private static BigInteger d = ModInverse(exp, phi);
           private static BigInteger encryptedMessage;
           private static BigInteger messageAsNumber;
           private static string message;


            private static BigInteger GenerateLargePrime(int bitLength)
            {
                using (var rng = new RNGCryptoServiceProvider())
                {
                    byte[] bytes = new byte[bitLength / 8 + 1];
                    BigInteger number;
                    do
                    {
                        rng.GetBytes(bytes);
                        number = new BigInteger(bytes);
                    }
                    while (!IsPrime(number));
                    return number;
                }

            }   

            
            private static bool IsPrime(BigInteger number)
            {
                if (number < 2) return false;
                for (BigInteger i = 2; i * i <= number; i++)
                {
                    if (number % i == 0) return false;
                }
                return true;
            }

            
            private static BigInteger ModInverse(BigInteger a, BigInteger m)
            {
                BigInteger m0 = m, t, q;
                BigInteger x0 = 0, x1 = 1;

                if (m == 1) return 0;

                while (a > 1)
                {
                    q = a / m;
                    t = m;

                    m = a % m;
                    a = t;
                    t = x0;

                    x0 = x1 - q * x0;
                    x1 = t;
                }

                if (x1 < 0)
                    x1 += m0;

                return x1;
            }
        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            message = textBox7.Text;
            messageAsNumber = new BigInteger(Encoding.UTF8.GetBytes(message));
            encryptedMessage = BigInteger.ModPow(messageAsNumber, exp, n);
            textBox8.Text = p.ToString();
            textBox1.Text = q.ToString();
            textBox2.Text = n.ToString();
            textBox4.Text = phi.ToString();
            textBox3.Text = exp.ToString();
            textBox9.Text = d.ToString();
            
            textBox6.Text = encryptedMessage.ToString();


        }
    }
}
