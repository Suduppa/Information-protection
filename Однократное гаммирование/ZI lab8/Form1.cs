using System;
using System.Text;

namespace ZI_lab8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Генерация гаммы случайных байтов заданной длины.
        /// </summary>
        /// <param name="length">Длина гаммы.</param>
        /// <returns>Массив байтов (гамма).</returns>
        private byte[] GenerateGamma(int length)
        {
            byte[] gamma = new byte[length];
            Random random = new Random();
            random.NextBytes(gamma); // Заполняем массив случайными байтами
            return gamma;
        }

        /// <summary>
        /// Шифрование текста методом одноразового блокнота (One-Time Pad).
        /// </summary>
        /// <param name="data">Массив байтов исходного текста.</param>
        /// <param name="gamma">Массив байтов гаммы (ключ).</param>
        /// <returns>Зашифрованный массив байтов.</returns>
        private byte[] EncryptWithOneTimePad(byte[] data, byte[] gamma)
        {
            byte[] result = new byte[data.Length];
            int blockSize = 4; // Используем блочную обработку

            for (int i = 0; i < data.Length; i += blockSize)
            {
                for (int j = 0; j < blockSize && (i + j) < data.Length; j++)
                {
                    result[i + j] = (byte)(data[i + j] ^ gamma[i + j]); // Побитовая операция XOR
                }
            }

            return result;
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Шифровать".
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string plaintext = textBox1.Text; 

                if (string.IsNullOrWhiteSpace(plaintext))
                {
                    MessageBox.Show("Введите текст для шифрования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

                
                byte[] gamma = GenerateGamma(plaintextBytes.Length);

                
                byte[] encryptedMessage = EncryptWithOneTimePad(plaintextBytes, gamma);

                
                richTextBox1.Text = Convert.ToBase64String(encryptedMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void Form1_Load(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void richTextBox1_TextChanged(object sender, EventArgs e) { }
    }
}
