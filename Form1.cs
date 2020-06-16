using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO.Ports;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        const byte SLAVE1_ADDRESS = 0x01;
        const byte SLAVE2_ADDRESS = 0x02;
        public Form1()
        {
            InitializeComponent();
        }
        
        private void comboBox1_Click(object sender, EventArgs e)
        {
            int num;
            comboBox1.Items.Clear();
            string[] ports = SerialPort.GetPortNames().OrderBy(a => a.Length > 3 && int.TryParse(a.Substring(3), out num) ? num : 0).ToArray();
            comboBox1.Items.AddRange(ports);
        }

        private void buttonOpenPort_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
                try
                {
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.Open();
                    buttonOpenPort.Text = "Close";
                    comboBox1.Enabled = false;

                    buttonFromSlave1.Visible = true;
                    buttonFromSlave2.Visible = true;
                    button1.Visible = true;
                }
                catch
                {
                    MessageBox.Show("Port " + comboBox1.Text + " is invalid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            else
            {
                serialPort1.Close();
                buttonOpenPort.Text = "Open";
                comboBox1.Enabled = true;

                buttonFromSlave1.Visible = false;
                buttonFromSlave2.Visible = false;
                
            }
        }


        public static ushort Compute_CRC8_ROHC(byte[] bytes)
        {
            const ushort generator = 0x17 << 3;
            ushort crc = 0xFF;

            foreach (byte b in bytes)
            {
                crc ^= (ushort)b;

                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 1) != 0)
                    {
                        crc = (ushort)((crc >> 1) ^ generator);
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }

            return crc;
        }

        private void buttonFromSlave1_Click(object sender, EventArgs e)
        {
            int errorFrame = -1;
            byte[] b1 = new byte[3];
            b1[0] = SLAVE1_ADDRESS;
            b1[1] = 0xB1;//команда Читання
            serialPort1.Write(b1, 0, 2);
            int packetsAmount = serialPort1.ReadByte();
            String message = "";
            for (int i = 0; i < packetsAmount; i++)
            {
                String packet = serialPort1.ReadLine();
                if (int.Parse(serialPort1.ReadLine()) == Compute_CRC8_ROHC(Encoding.ASCII.GetBytes(packet)))
                {
                    message += packet;
                }
                else
                {
                    errorFrame = i;
                }
            }
            if (errorFrame != -1) message = "error in frame" + errorFrame.ToString();
            textBox2.Text = message;

            String res = "";
            int number = (int)Convert.ToDouble(textBox2.Text);
            if (number <= 0)
            {
                res = "Рибам холодно!";
            }
            else if (number > 0 && number <= 15)
            {
                res = "Нормусік!";
            }
            else if (number > 15 && number <= 40)
            {
                res = "Ше живі!";
            }
            else
            {
                res = "Рибки здохли :(";
            }

            textBox4.Text = res;
        }

        private void buttonFromSlave2_Click(object sender, EventArgs e)
        {
            int errorFrame = -1;
            byte[] b1 = new byte[3];
            b1[0] = SLAVE1_ADDRESS;
            b1[1] = 0xB1;//команда Читання
            serialPort1.Write(b1, 0, 2);
            int packetsAmount = serialPort1.ReadByte();
            String message = "";
            for (int i = 0; i < packetsAmount; i++)
            {
                String packet = serialPort1.ReadLine();
                if (int.Parse(serialPort1.ReadLine()) == Compute_CRC8_ROHC(Encoding.ASCII.GetBytes(packet)))
                {
                    message += packet;
                }
                else
                {
                    errorFrame = i;
                }
            }
            if (errorFrame != -1) message = "error in frame" + errorFrame.ToString();
            textBox3.Text = message;

            String res = "";
            int number = (int)Convert.ToDouble(textBox3.Text);
            if (number <= 0)
            {
                res = "Рибам холодно!";
            }
            else if (number > 0 && number <= 15)
            {
                res = "Нормусік!";
            }
            else if (number > 15 && number <= 40)
            {
                res = "Ше живі!";
            }
            else
            {
                res = "Рибки здохли :(";
            }

            textBox5.Text = res;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int errorFrame = -1;
            byte[] b1 = new byte[3];
            b1[0] = SLAVE2_ADDRESS;
            b1[1] = 0xC1;//команда Читання
            serialPort1.Write(b1, 0, 2);
            int packetsAmount = serialPort1.ReadByte();
            String message = "";
            for (int i = 0; i < packetsAmount; i++)
            {
                String packet = serialPort1.ReadLine();
                if (int.Parse(serialPort1.ReadLine()) == Compute_CRC8_ROHC(Encoding.ASCII.GetBytes(packet)))
                {
                    message += packet;
                }
                else
                {
                    errorFrame = i;
                }
            }
            if (errorFrame != -1) message = "error in frame" + errorFrame.ToString();
            textBox8.Text = message;
        }
    }
}
