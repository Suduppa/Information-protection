using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ZI_lab6
{
    public partial class Form1 : Form
    {
        private string path = ""; // ���� � ���������� �����

        public Form1()
        {
            InitializeComponent();
        }

        // ����� ������ RLE
        public byte[] CompressRLE(byte[] input)
        {
            using (MemoryStream compressedStream = new MemoryStream())
            {
                for (int i = 0; i < input.Length; i++)
                {
                    byte currentByte = input[i];
                    int runLength = 1;

                    while (i + 1 < input.Length && input[i + 1] == currentByte && runLength < 255)
                    {
                        runLength++;
                        i++;
                    }

                    // ���������� ���������� �������� � ��� ������
                    compressedStream.WriteByte((byte)runLength);
                    compressedStream.WriteByte(currentByte);
                }
                return compressedStream.ToArray();
            }
        }

        // ����� ������ LZW
        public List<int> CompressLZW(byte[] input)
        {
            // ������������� ������� ��� ASCII ��������
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int i = 0; i < 256; i++)
            {
                dictionary.Add(((char)i).ToString(), i);
            }

            string currentStr = "";
            List<int> compressed = new List<int>();
            int dictSize = 256;

            foreach (byte b in input)
            {
                char currentChar = (char)b;
                string newStr = currentStr + currentChar;

                // ���� ���������� ��� � �������, ����������
                if (dictionary.ContainsKey(newStr))
                {
                    currentStr = newStr;
                }
                else
                {
                    // ��������� ������ �� ������� � ��������� ����� ����������
                    compressed.Add(dictionary[currentStr]);
                    dictionary[newStr] = dictSize++;
                    currentStr = currentChar.ToString();
                }
            }

            // ���������� ���������� ������
            if (!string.IsNullOrEmpty(currentStr))
            {
                compressed.Add(dictionary[currentStr]);
            }

            return compressed;
        }

        // ���������� ��� ������ "�����"
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("�������� ���� ����� ������� ������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // ������ ����������� �����
                byte[] inputText = File.ReadAllBytes(path);
                richTextBox1.Text = Encoding.UTF8.GetString(inputText);

                // ������ �����
                var rleCompressed = CompressRLE(inputText);
                var lzwCompressed = CompressLZW(rleCompressed);

                // ������ ������� �����
                string compressedFilePath = path + ".compressed";
                using (BinaryWriter writer = new BinaryWriter(File.Open(compressedFilePath, FileMode.Create)))
                {
                    foreach (int c in lzwCompressed)
                    {
                        writer.Write((ushort)c);
                    }
                }

                // ����������� ������ ������ � richTextBox2
                byte[] compressedFile = File.ReadAllBytes(compressedFilePath);
                StringBuilder compressedData = new StringBuilder();
                foreach (byte c in compressedFile)
                {
                    compressedData.Append($"\\{c}");
                }
                richTextBox2.Text = compressedData.ToString();

                MessageBox.Show($"������ ���������! ������ ���� �������� ���: {compressedFilePath}", "�������", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��������� ������ ��� ������: {ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ���������� ��� ������ ������ �����
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "��������� ����� (*.txt)|*.txt|��� ����� (*.*)|*.*";
            openFileDialog.Title = "�������� ����";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog.FileName;
                textBox1.Text = path; // ����������� ���� � textBox1
            }
            else
            {
                MessageBox.Show("���� �� ��� ������.", "��������������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ���������� ������� ��������� ������ richTextBox1
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // ����� ����� ���������� ������� ��������� ������, ���� ��� �����������
        }

        // ���������� ������� ��������� ������ richTextBox2
        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            // ����� ����� ���������� ������� ��������� ������, ���� ��� �����������
        }

        // ���������� ��������� ������ � textBox1
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // ����� ����� ���������� ��������� ���� �����
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // ����� �������� ��� �������� �����
        }

    }
}
