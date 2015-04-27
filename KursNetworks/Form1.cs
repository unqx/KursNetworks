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
            TransmittingWorker.RunWorkerAsync();
            DownloadButton.Enabled = false;
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
                        if (!DataLink.FileRecieving && !DataLink.FileSending)
                            UpdateButton.Enabled = true;
                        else
                            UpdateButton.Enabled = false;
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
                        label1.Invoke((MethodInvoker)delegate
                        {
                            label1.Text = "Подключен к порту " + PhysLayer.GetPortName();
                        });

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
            // Запускаем в основном потоке, т.к. не занимает много времени.
            listBox1.Items.Clear();
            textBox1.Text += "\r\nLoading files...";
            int i = 0;

            // Даем время отработать ком портам
            while (DataLink.filesUpdated != true)
            {
                ++i;
                textBox1.Text += ".";
                Thread.Sleep(50);
                if(i == 10)
                    break;

            }
            textBox1.Text += "\r\n";

            //Если быбла ошибка, то выводим messagebox
            if (DataLink.filesUpdated)
            {
                listBox1.Items.AddRange(DataLink.files);
                DataLink.filesUpdated = false;
            }
            else
                MessageBox.Show("Не удалось обновить список файлов!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.Text != "")
                DownloadButton.Enabled = true;
            else
                DownloadButton.Enabled = false;
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            DataLink.FileRecieving = true;
            DataLink.FileRecievingName = listBox1.Text;
            DataLink.DownloadRequest(listBox1.Text);
        }

        private void TransmittingWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (!DataLink.FileSending && !DataLink.FileRecieving)
                {
                    if (!DataLink.SendQueue.IsEmpty)
                    {
                        File F;
                        if (DataLink.SendQueue.TryDequeue(out F))
                        {
                            DataLink.FileSending = true;
                            DataLink.StartSendingFile(F);

                            /****** установка прогресс-бара ******/

                            progressBar1.Invoke((MethodInvoker)delegate
                            {
                                progressBar1.Maximum = (int)(F.Size / 1024);
                            });

                            /**************************************/

                            FileStream Stream = new FileStream(F.Name, FileMode.Open, FileAccess.Read);
                            byte R;
                            byte[] buffer = new byte[1024];
                            
                            int counter = 0; // счетчик ошибок

                            while(DataLink.FileSending)
                            {
                                if(PhysLayer.Responses.TryDequeue(out R))
                                {
                                    if (R == Convert.ToByte('A'))
                                    {
                                        counter = 0;
                                        try
                                        {
                                            int BytesRead = Stream.Read(buffer, 0, buffer.Length);
                                            if (BytesRead > 0)
                                            {
                                                byte[] clean = new byte[BytesRead];
                                                for (int i = 0; i < BytesRead; i++ )
                                                {
                                                    clean[i] = buffer[i];
                                                }

                                                int step = clean.Length;

                                                clean = DataLink.pack('I', clean);
                                                clean = DataLink.EncodeFrame(clean);
                                                PhysLayer.Write(clean);

                                                progressBar1.Invoke((MethodInvoker)delegate
                                                {
                                                    progressBar1.Step = step / 1024;
                                                    progressBar1.PerformStep();
                                                });
                                            }
 
                                            else
                                            {
                                                Stream.Close();
                                                DataLink.EOF();
                                                DataLink.FileSending = false;

                                                progressBar1.Invoke((MethodInvoker)delegate
                                                {
                                                    progressBar1.Value = 0;
                                                });
                                            }
                                        }

                                        catch(ArgumentException)
                                        {
                                            MessageBox.Show("ISKLUCHENIE");
                                        }
                                       
                                    }

                                     if (R == Convert.ToByte('N'))
                                     {
                                         counter++;
                                         PhysLayer.Write(buffer);
                                     }
                                }
                            }
                        }
                    }
                }

                if (DataLink.FileRecieving)
                {
                    string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string fullPath = desktop + "\\(NEW)" + DataLink.FileRecievingName;

                    FileStream Stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                    MessageBox.Show(Convert.ToString(DataLink.FileRecievingSize), "SUKA!", MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);

                    progressBar1.Invoke((MethodInvoker)delegate
                    {
                        progressBar1.Maximum = DataLink.FileRecievingSize / 1024;

                    });

                    while(true)
                    {
                        byte[] result;
                        if(PhysLayer.FramesRecieved.TryDequeue(out result))
                        {
                            if(Encoding.Default.GetString(result) == "EOF")
                            {
                                Stream.Close();
                                DataLink.FileRecieving = false;

                                progressBar1.Invoke((MethodInvoker)delegate
                                {
                                    progressBar1.Value = 0;
                                });

                                break;
                            }

                            try
                            {
                                Stream.Write(result, 0, result.Length);

                                progressBar1.Invoke((MethodInvoker)delegate
                                {
                                    progressBar1.Step = result.Length / 1024;
                                    progressBar1.PerformStep();
                                });
                            }

                            catch(IOException)
                            {
                                MessageBox.Show("NEKUDA PISAT'");
                            }
                           
                        }
                    }
                }

                Thread.Sleep(1000);
             
            }

        }

    }
}
