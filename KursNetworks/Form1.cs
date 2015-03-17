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
        static PhysLayer Layer = new PhysLayer();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string port in PhysLayer.scanPorts())
            {
                comboBox1.Items.Add(port);
            }

            button1.Enabled = false;
            
        }

        // Кнопка "выбрать"
        private void button1_Click(object sender, EventArgs e)
        {
            var connection = new SerialPort();
            connection.PortName = comboBox1.Text;
            connection.Open();
            textBox1.Text += "Established connection on " + connection.PortName;
            button1.Enabled = false;
            comboBox1.Enabled = false;
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            if (PhysLayer.IsOpen())
                textBox1.Text += "TRUE!";
            else
                textBox1.Text += "FALSE!";

        }

        // Toogle для кнопки "выбрать"
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox1.Text != "")
                button1.Enabled = true;
            else
                button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ConnectionSettings SettingsForm = new ConnectionSettings();
            SettingsForm.ShowDialog();
        }
    }
}
