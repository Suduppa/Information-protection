using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vsh2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static string Encrypt(string message, string key)
        {
            // Удаляем пробелы
            message = message.Replace(" ", "");

            int columns = key.Length;
            int rows = (int)Math.Ceiling((double)message.Length / columns); 

            // Заполняем таблицу
            char[,] table = new char[rows, columns];
            int charIndex = 0;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    if (charIndex < message.Length)
                    {
                        table[r, c] = message[charIndex++];
                    }
                    else
                    {
                        table[r, c] = ' '; 
                    }
                }
            }

            
            int[] keyOrder = GetKeyOrder(key);

            // Читаем по столбцам в новом порядке
            string encryptedMessage = "";
            for (int i = 0; i < columns; i++)
            {
                int col = keyOrder[i];
                for (int r = 0; r < rows; r++)
                {
                    encryptedMessage += table[r, col];
                }
            }

            return encryptedMessage;
        }

        //столбцы по ключу
        static int[] GetKeyOrder(string key)
        {
            char[] keyChars = key.ToArray();
            int[] order = new int[key.Length];

            //алфавит
            char[] sortedKey = keyChars.OrderBy(c => c).ToArray();
            for (int i = 0; i < key.Length; i++)
            {
                order[i] = Array.IndexOf(keyChars, sortedKey[i]);
                keyChars[order[i]] = '\0'; //дубли
            }

            return order;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            string message = textBox1.Text; 
        
            string key = textBox2.Text; 

            //Console.WriteLine("Исходное сообщение: " + message);

            textBox3.Text = Encrypt(message, key);
            //string encryptedMessage = Encrypt(message, key);
            //Console.WriteLine("Зашифрованное сообщение: " + encryptedMessage);


        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
