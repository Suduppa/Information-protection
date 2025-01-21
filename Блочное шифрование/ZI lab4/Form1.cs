using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ZI_lab4
{
    public partial class Form1 : Form
    {
        uint encryptionKey = 0xA1B2C3D4; // Ключ

        public Form1()
        {
            InitializeComponent();
        }

        // Метод шифрования
        void Encrypt(string outputFile)
        {
            byte[] fileBytes = Encoding.UTF8.GetBytes(textBox1.Text);
            byte[] encryptedBytes = new byte[fileBytes.Length];

            for (int i = 0; i < fileBytes.Length; i += 2) //блоками по 2 байта
            {
                ushort block = 0; //чтение
                if (i + 1 < fileBytes.Length)
                    block = BitConverter.ToUInt16(fileBytes, i);
                else
                    block = fileBytes[i];

                ushort encryptedBlock = (ushort)(block ^ (encryptionKey & 0xFFFF));

                byte[] encryptedBytesBlock = BitConverter.GetBytes(encryptedBlock);
                encryptedBytes[i] = encryptedBytesBlock[0];
                if (i + 1 < fileBytes.Length)
                    encryptedBytes[i + 1] = encryptedBytesBlock[1];
            }

            textBox2.Text = Encoding.UTF8.GetString(encryptedBytes);
            File.WriteAllBytes(outputFile, encryptedBytes);
        }

        // Метод расшифрования
        void Decrypt(string inputFile, string outputFile)
        {
            byte[] fileBytes = File.ReadAllBytes(inputFile);
            byte[] decryptedBytes = new byte[fileBytes.Length];

            for (int i = 0; i < fileBytes.Length; i += 2)
            {
                ushort block = 0;
                if (i + 1 < fileBytes.Length)
                    block = BitConverter.ToUInt16(fileBytes, i);
                else
                    block = fileBytes[i];

                ushort decryptedBlock = (ushort)(block ^ (encryptionKey & 0xFFFF));

                byte[] decryptedBytesBlock = BitConverter.GetBytes(decryptedBlock);
                decryptedBytes[i] = decryptedBytesBlock[0];
                if (i + 1 < fileBytes.Length)
                    decryptedBytes[i + 1] = decryptedBytesBlock[1];
            }

            textBox3.Text = Encoding.UTF8.GetString(decryptedBytes);
            File.WriteAllBytes(outputFile, decryptedBytes);
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string encryptedFile = @"D:\Защита информации\encrypted.txt";
                Encrypt(encryptedFile);
                MessageBox.Show("Шифрование выполнено! Данные сохранены в файл: " + encryptedFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при шифровании: " + ex.Message);
            }
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string encryptedFile = @"D:\Защита информации\encrypted.txt";
                string decryptedFile = @"D:\Защита информации\decrypted.txt";
                Decrypt(encryptedFile, decryptedFile);
                MessageBox.Show("Расшифрование выполнено! Данные сохранены в файл: " + decryptedFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при расшифровании: " + ex.Message);
            }
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
