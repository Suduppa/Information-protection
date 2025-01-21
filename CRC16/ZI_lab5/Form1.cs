using System.Text;

namespace ZI_lab5
{
    public partial class Form1 : Form
    {
        private const ushort POLYNOMIAL = 0x8005; // Полином
        private const ushort INIT_CRC = 0x0;    
        private ushort calculatedCRC;          
        private byte[] messageWithCRC = Array.Empty<byte>(); // Сообщение с CRC

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                MessageBox.Show("Сообщение не может быть пустым");
                return;
            }

            richTextBox2.Text = string.Empty; 
            byte[] message = Encoding.ASCII.GetBytes(richTextBox1.Text); 

            
            messageWithCRC = AppendCRC(message);

            
            foreach (byte b in messageWithCRC)
            {
                richTextBox2.Text += $"{Convert.ToString(b, 2).PadLeft(8, '0')} "; 
            }

            
            label4.Text = CheckCRC(messageWithCRC).ToString();
            textBox1.Text = Convert.ToString(calculatedCRC, 2).PadLeft(16, '0'); // CRC в двоичке
        }

        // Проверка
        private void button2_Click(object sender, EventArgs e)
        {
            if (messageWithCRC == null || messageWithCRC.Length == 0)
            {
                MessageBox.Show("Сначала сгенерируйте сообщение с CRC, нажав кнопку 'Старт'");
                return;
            }

            label4.Text = CheckCRC(messageWithCRC).ToString();
        }

        // Метод вычисления CRC-16
        private ushort ComputeCRC16(byte[] data)
        {
            ushort crc = INIT_CRC;

            foreach (byte b in data)
            {
                crc ^= (ushort)(b << 8);
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x8000) != 0)
                        crc = (ushort)((crc << 1) ^ POLYNOMIAL);
                    else
                        crc <<= 1;
                }
            }

            return crc;
        }

        private byte[] AppendCRC(byte[] data)
        {
            ushort crc = ComputeCRC16(data);
            byte[] result = new byte[data.Length + 2];

            Array.Copy(data, result, data.Length);

            result[result.Length - 2] = (byte)(crc >> 8);
            result[result.Length - 1] = (byte)(crc & 0xFF);

            return result;
        }

        // Метод проверки CRC
        private bool CheckCRC(byte[] dataWithCRC)
        {
            if (dataWithCRC.Length < 2)
                return false;

            byte[] data = new byte[dataWithCRC.Length - 2];
            Array.Copy(dataWithCRC, data, data.Length);
            ushort receivedCRC = (ushort)((dataWithCRC[dataWithCRC.Length - 2] << 8) |
                                          dataWithCRC[dataWithCRC.Length - 1]);

            calculatedCRC = ComputeCRC16(data);

            return receivedCRC == calculatedCRC;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (messageWithCRC == null || messageWithCRC.Length == 0)
            {
                label4.Text = "False";
                return;
            }

            try
            {
                // Пробуем новый CRC
                ushort newCRC = Convert.ToUInt16(textBox1.Text, 2);

                
                messageWithCRC[messageWithCRC.Length - 2] = (byte)(newCRC >> 8);
                messageWithCRC[messageWithCRC.Length - 1] = (byte)(newCRC & 0xFF);

                // Проверка
                label4.Text = CheckCRC(messageWithCRC).ToString();
            }
            catch
            {
                label4.Text = "False";
            }
        }
    }
}
