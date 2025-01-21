using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ZI_lab7
{
    public partial class Form1 : Form
    {
        private const uint encryptionKey = 0xA1B2C3D4;

        public Form1()
        {
            InitializeComponent();
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16]; // 128-битная соль
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(salt);

            return salt;
        }

        private byte[] HashPasswordWithBlockCipher(string password, byte[] salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combined = new byte[salt.Length + passwordBytes.Length];

            Buffer.BlockCopy(salt, 0, combined, 0, salt.Length);
            Buffer.BlockCopy(passwordBytes, 0, combined, salt.Length, passwordBytes.Length);

            byte[] hash = Encrypt(combined);

            return hash;
        }

        private byte[] Encrypt(byte[] data)
        {
            byte[] encryptedBytes = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                ushort block = 0;
                if (i + 1 < data.Length)
                    block = BitConverter.ToUInt16(data, i);
                else
                    block = data[i];

                ushort encryptedBlock = (ushort)(block ^ (encryptionKey & 0xFFFF));

                byte[] encryptedBytesBlock = BitConverter.GetBytes(encryptedBlock);
                encryptedBytes[i] = encryptedBytesBlock[0];
                if (i + 1 < data.Length)
                    encryptedBytes[i + 1] = encryptedBytesBlock[1];
            }

            return encryptedBytes;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string password = textBox1.Text;

           
            byte[] salt = GenerateSalt();

            
            byte[] hash = HashPasswordWithBlockCipher(password, salt);

            
            richTextBox1.Text = Convert.ToBase64String(hash);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
