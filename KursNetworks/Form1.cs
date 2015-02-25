using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace KursNetworks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var connection = new SerialPort();
            connection.PortName = comboBox1.Text;
            connection.Open();
            button1.Enabled = false;
            comboBox1.Enabled = false;
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            var connection = new SerialPort();
            connection.PortName = comboBox1.Text;
            textBox1.Text += connection.PortName;
        }
    }
}
