using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Deffe_Hellman
{
    public partial class Form1 : Form
    {

       /* 16)	Выполнить шифрование строки исходного текста, методом Диффи-Хеллмана, используя в качестве x и y
            простые числа с разрядностью не меньшей двенадцати, выполнив условие случайности x и y для каждого
            нового шифрования и используя в алгоритме шифрования функцию тангенса.*/


        private BigInteger privateKeyX;
        private BigInteger privateKeyY;
        private BigInteger publicKeyX;
        private BigInteger publicKeyY;
        private BigInteger sharedSecretX;
        private BigInteger sharedSecretY;
        private string originalText;
        private string encryptedText;

        public Form1()
        {
            InitializeComponent();
        }

        private static int bitLength = 12 * 4; 
        private static BigInteger prime = GeneratePrimeNumber(bitLength);
        private static BigInteger generator = 2; 

     
        public static BigInteger Sqrt(BigInteger number)
        {
            if (number == 0) return 0;
            if (number > 0)
            {
                BigInteger n = 0, p = number / 2 + 1;
                while (p < n || n == 0)
                {
                    n = p;
                    p = (number / p + p) / 2;
                }
                return p;
            }
            throw new ArithmeticException("Нельзя вычислить корень из отрицательного числа");
        }

        public static BigInteger GeneratePrimeNumber(int bitLength)
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[bitLength / 8];
                BigInteger prime;
                do
                {
                    rng.GetBytes(bytes);
                    prime = new BigInteger(bytes);
                    prime = BigInteger.Abs(prime); 
                }
                while (!IsPrime(prime));

                return prime;
            }
        }

        
        public static bool IsPrime(BigInteger number)
        {
            if (number < 2)
                return false;
            if (number % 2 == 0 && number != 2)
                return false;

            BigInteger sqrtNumber = Sqrt(number);
            for (BigInteger i = 3; i <= sqrtNumber; i += 2)
            {
                if (number % i == 0)
                    return false;
            }
            return true;
        }

        // Алгоритм Диффи-Хеллмана для генерации общего секрета
        public static BigInteger DiffieHellman(BigInteger prime, BigInteger generator, BigInteger privateKey)
        {
            return BigInteger.ModPow(generator, privateKey, prime);
        }

        //тангенса
        public static string EncryptString(string input, BigInteger secretKey)
        {
            StringBuilder encrypted = new StringBuilder();

            foreach (char c in input)
            {
                double tangentialValue = Math.Tan((int)c + (int)(secretKey % 256));
                encrypted.Append((int)(tangentialValue * 1000));
                encrypted.Append(" ");
            }

            return encrypted.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            originalText = textBox3.Text;

           
            privateKeyX = GeneratePrimeNumber(bitLength);
            privateKeyY = GeneratePrimeNumber(bitLength);

           
            publicKeyX = DiffieHellman(prime, generator, privateKeyX);
            publicKeyY = DiffieHellman(prime, generator, privateKeyY);

           
            sharedSecretX = DiffieHellman(prime, publicKeyY, privateKeyX);
            sharedSecretY = DiffieHellman(prime, publicKeyX, privateKeyY);

         
            encryptedText = EncryptString(originalText, sharedSecretX);

           
            textBox1.Text = privateKeyX.ToString();
            textBox2.Text = privateKeyY.ToString();
            textBox4.Text = encryptedText;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
       
        }

        private void label2_Click(object sender, EventArgs e)
        {
       
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
         
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
           
        }
    }
}
