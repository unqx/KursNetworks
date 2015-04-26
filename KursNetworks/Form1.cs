using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
            {
                textBox1.Text += PhysLayer.GetDataBits() + " " + PhysLayer.GetSpeed() + " / " + PhysLayer.GetPortName() + "\r\n" + Convert.ToString(PhysLayer.DsrSignal());
            }
                
            else
            {
                textBox1.Text += "FALSE\r\n";
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
                if (!prev)
                    textBox1.Text += "Etsablished connection via " + PhysLayer.GetPortName() + " with parameters: speed = " + PhysLayer.GetSpeed() + "\r\n";
                textBox1.SelectionStart = textBox1.TextLength;
                textBox1.ScrollToCaret();
            }
            
            else
            {
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

                    UpdateButton.Invoke((MethodInvoker)delegate
                    {
                        UpdateButton.Enabled = true;
                    });

                    // Если есть соединение логическое, то пишем название порта к которому подключены
                    if (PhysLayer.PortReciever != "" && DataLink.Connection)
                    {
                        label1.Invoke((MethodInvoker)delegate
                        {
                            label1.Text = "Подключение через " + PhysLayer.GetPortName() + " к " + PhysLayer.PortReciever;
                        });
                    }
                    else
                    {
                        if (!DataLink.Connection)
                        {
                            DataLink.EstablishConnection();
                            DataLink.Connection = true;
                        }
                    }
                }
                else 
                {
                    label3.Invoke((MethodInvoker)delegate
                    {
                        label3.Text = "Соединение отсутствует";
                        label3.ForeColor = Color.Red;
                    });

                    UpdateButton.Invoke((MethodInvoker)delegate
                    {
                        UpdateButton.Enabled = false;
                    });

                    label1.Invoke((MethodInvoker)delegate
                    {
                        if (PhysLayer.IsOpen())
                            label1.Text = "Подключен к порту " + PhysLayer.GetPortName();
                        else
                            label1.Text = "Порт закрыт";

                    });

                    DataLink.Connection = false;
                    PhysLayer.PortReciever = "";
                 }

                System.Threading.Thread.Sleep(1000);
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            DataLink.RequestAvailableFiles();
            ChangeFilenames();
        }

        private void ChangeFilenames()
        {
            listBox1.Items.Clear();
            textBox1.Text += "\r\nLoading files...";
            while (DataLink.filesUpdated != true)
            {
                textBox1.Text += ".";
                Thread.Sleep(50);

            }
            textBox1.Text += "\r\n";
            listBox1.Items.AddRange(DataLink.files);
            DataLink.filesUpdated = false;
           
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            byte a = 5;
            byte b = Hamming.Code(a);
            MessageBox.Show(Convert.ToString(a, 2).PadLeft(8, '0'));
            MessageBox.Show(Convert.ToString(b, 2).PadLeft(8, '0'));
            try
            {
               b = Hamming.Decode(b);
            }

            catch(Exception)
            {
                MessageBox.Show("NE MOGU DECODE SDELAT BRATISHKA");
            }
            MessageBox.Show(Convert.ToString(b, 2).PadLeft(8, '0'));
        }

    }
}
