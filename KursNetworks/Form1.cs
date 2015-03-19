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
using System.IO;

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
           
        }

       
        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            if (PhysLayer.IsOpen())
                textBox1.Text += PhysLayer.GetDataBits() + " " + PhysLayer.GetSpeed() + " / " + PhysLayer.GetPortName() + "\r\n";
            else
                textBox1.Text += "FALSE\r\n";

        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            bool prev = PhysLayer.IsOpen();
            string port = PhysLayer.GetPortName();
            ConnectionSettings SettingsForm = new ConnectionSettings();
            SettingsForm.ShowDialog();

            // Чисто для прикола)))
            if (PhysLayer.IsOpen())
            {
                label1.Text = "Подключен к порту " + PhysLayer.GetPortName();
                if (!prev)
                    textBox1.Text += "Etsablished connection via " + PhysLayer.GetPortName() + " with parameters: speed = " + PhysLayer.GetSpeed() + "\r\n";
                textBox1.SelectionStart = textBox1.TextLength;
                textBox1.ScrollToCaret();
            }
            
            else
            {
                label1.Text = "Нет подключения к COM-порту!";
                if(prev)
                    textBox1.Text += "Connection via " + port + " was dropped.\r\n";
                textBox1.SelectionStart = textBox1.TextLength;
                textBox1.ScrollToCaret();
            }

        }

        private void FileChoice_Click(object sender, EventArgs e)
        {
            OpenFileDialog search = new OpenFileDialog();
            DialogResult result = search.ShowDialog();
        }
    }
}
