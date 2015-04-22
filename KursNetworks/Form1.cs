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
        public static bool connection = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConnectionWorker.RunWorkerAsync();
        }

       
        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            if (PhysLayer.IsOpen())
                textBox1.Text += PhysLayer.GetDataBits() + " " + PhysLayer.GetSpeed() + " / " + PhysLayer.GetPortName() + "\r\n" + Convert.ToString(PhysLayer.DsrSignal());
            else
            {
                textBox1.Text += "FALSE\r\n";
                DataLink.filesAvailableRequest();
            }


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

        private void ConnectionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                if (PhysLayer.DsrSignal())
                {
                    label3.Invoke((MethodInvoker)delegate
                    {
                        label3.Text = "Соединение активно";
                        label3.ForeColor = Color.Green;
                    });

                }
                else 
                {
                    label3.Invoke((MethodInvoker)delegate
                    {
                        label3.Text = "Соединение отсутствует";
                        label3.ForeColor = Color.Red;
                    });
                }

                System.Threading.Thread.Sleep(1000);
            }
        }


    }
}
