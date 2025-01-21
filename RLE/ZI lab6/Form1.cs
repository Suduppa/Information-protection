using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ZI_lab6
{
    public partial class Form1 : Form
    {
        private string path = ""; // Путь к выбранному файлу

        public Form1()
        {
            InitializeComponent();
        }

        // Метод сжатия RLE
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

                    // Записываем количество повторов и сам символ
                    compressedStream.WriteByte((byte)runLength);
                    compressedStream.WriteByte(currentByte);
                }
                return compressedStream.ToArray();
            }
        }

        // Метод сжатия LZW
        public List<int> CompressLZW(byte[] input)
        {
            // Инициализация словаря для ASCII символов
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

                // Если комбинация уже в словаре, продолжаем
                if (dictionary.ContainsKey(newStr))
                {
                    currentStr = newStr;
                }
                else
                {
                    // Сохраняем индекс из словаря и добавляем новую комбинацию
                    compressed.Add(dictionary[currentStr]);
                    dictionary[newStr] = dictSize++;
                    currentStr = currentChar.ToString();
                }
            }

            // Записываем оставшийся символ
            if (!string.IsNullOrEmpty(currentStr))
            {
                compressed.Add(dictionary[currentStr]);
            }

            return compressed;
        }

        // Обработчик для кнопки "Старт"
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Выберите файл перед началом сжатия.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Чтение содержимого файла
                byte[] inputText = File.ReadAllBytes(path);
                richTextBox1.Text = Encoding.UTF8.GetString(inputText);

                // Сжатие файла
                var rleCompressed = CompressRLE(inputText);
                var lzwCompressed = CompressLZW(rleCompressed);

                // Запись сжатого файла
                string compressedFilePath = path + ".compressed";
                using (BinaryWriter writer = new BinaryWriter(File.Open(compressedFilePath, FileMode.Create)))
                {
                    foreach (int c in lzwCompressed)
                    {
                        writer.Write((ushort)c);
                    }
                }

                // Отображение сжатых данных в richTextBox2
                byte[] compressedFile = File.ReadAllBytes(compressedFilePath);
                StringBuilder compressedData = new StringBuilder();
                foreach (byte c in compressedFile)
                {
                    compressedData.Append($"\\{c}");
                }
                richTextBox2.Text = compressedData.ToString();

                MessageBox.Show($"Сжатие завершено! Сжатый файл сохранен как: {compressedFilePath}", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при сжатии: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Обработчик для кнопки выбора файла
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            openFileDialog.Title = "Выберите файл";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog.FileName;
                textBox1.Text = path; // Отображение пути в textBox1
            }
            else
            {
                MessageBox.Show("Файл не был выбран.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Обработчик события изменения текста richTextBox1
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Здесь можно обработать событие изменения текста, если это потребуется
        }

        // Обработчик события изменения текста richTextBox2
        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            // Здесь можно обработать событие изменения текста, если это потребуется
        }

        // Обработчик изменения текста в textBox1
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Здесь можно обработать изменение пути файла
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Метод заглушка для загрузки формы
        }

    }
}
