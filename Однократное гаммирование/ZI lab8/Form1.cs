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
        /// ��������� ����� ��������� ������ �������� �����.
        /// </summary>
        /// <param name="length">����� �����.</param>
        /// <returns>������ ������ (�����).</returns>
        private byte[] GenerateGamma(int length)
        {
            byte[] gamma = new byte[length];
            Random random = new Random();
            random.NextBytes(gamma); // ��������� ������ ���������� �������
            return gamma;
        }

        /// <summary>
        /// ���������� ������ ������� ������������ �������� (One-Time Pad).
        /// </summary>
        /// <param name="data">������ ������ ��������� ������.</param>
        /// <param name="gamma">������ ������ ����� (����).</param>
        /// <returns>������������� ������ ������.</returns>
        private byte[] EncryptWithOneTimePad(byte[] data, byte[] gamma)
        {
            byte[] result = new byte[data.Length];
            int blockSize = 4; // ���������� ������� ���������

            for (int i = 0; i < data.Length; i += blockSize)
            {
                for (int j = 0; j < blockSize && (i + j) < data.Length; j++)
                {
                    result[i + j] = (byte)(data[i + j] ^ gamma[i + j]); // ��������� �������� XOR
                }
            }

            return result;
        }

        /// <summary>
        /// ���������� ������� ������ "���������".
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string plaintext = textBox1.Text; 

                if (string.IsNullOrWhiteSpace(plaintext))
                {
                    MessageBox.Show("������� ����� ��� ����������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);

                
                byte[] gamma = GenerateGamma(plaintextBytes.Length);

                
                byte[] encryptedMessage = EncryptWithOneTimePad(plaintextBytes, gamma);

                
                richTextBox1.Text = Convert.ToBase64String(encryptedMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��������� ������: {ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void Form1_Load(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void richTextBox1_TextChanged(object sender, EventArgs e) { }
    }
}
