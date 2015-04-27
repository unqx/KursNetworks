using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using System.IO;

namespace KursNetworks
{
    static class PhysLayer
    {
        private static SerialPort serialPort = new SerialPort();
        public static string PortReciever = "";

        public static ConcurrentQueue<byte[]> FramesRecieved = new ConcurrentQueue<byte[]>();
        public static ConcurrentQueue<byte> Responses = new ConcurrentQueue<byte>();

        public static bool DsrSignal()
        {
            if(IsOpen())
                return serialPort.DsrHolding;
            return false;
        }

        public static void OpenPort(string name, int rate, int dataBits, StopBits S, Parity P)
        {
            serialPort.PortName = name;
            serialPort.BaudRate = rate;
            serialPort.DataBits = dataBits;
            serialPort.StopBits = S;
            serialPort.Parity = P;
            serialPort.Handshake = Handshake.None;
            serialPort.DtrEnable = true;
            serialPort.RtsEnable = false;
            serialPort.ReadTimeout = 500;
            serialPort.WriteTimeout = 500;

            try
            {
                serialPort.Open();
                serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            }

            catch(System.UnauthorizedAccessException)
            {
                const string message = "Доступ к COM-порту закрыт!";
                const string caption = "Ошибка";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch(System.IO.IOException)
            {
                const string message = "Параметры выбраны неверно!\r\nПопробуйте выбрать другие стоп-биты!";
                const string caption = "Ошибка";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private static void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            List<byte> recievedList = new List<byte>();

            while (serialPort.BytesToRead > 0)
            {
                byte a = Convert.ToByte(serialPort.ReadByte());
                recievedList.Add(a);
            }

            byte[] recievedArray = recievedList.ToArray();
            if(recievedArray.Length != 0)
                DataLink.Analyze(recievedArray);
                
        }


        public static void Write(byte[] arr)
        {
            serialPort.Write(arr, 0, arr.Length);
        }


         /****************** Здесь изи функции ***********************/

        // Скан портов
        public static string[] scanPorts()
        {
            return SerialPort.GetPortNames();
        }


        // Дропнуть соединение
        public static void DropConnection()
        {
            serialPort.DataReceived -= new SerialDataReceivedEventHandler(serialPort_DataReceived);
            if(DataLink.FileRecieving || DataLink.FileSending)
                MessageBox.Show("Ошибка передачи!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            PhysLayer.ShutDown();
            serialPort.Close();
        }

        public static void ShutDown()
        {
            DataLink.FileRecieving = false;
            DataLink.FileSending = false;
            PhysLayer.FramesRecieved = new ConcurrentQueue<byte[]>();
            PhysLayer.Responses = new ConcurrentQueue<byte>();
            DataLink.SendQueue = new ConcurrentQueue<File>();
        }

        // Открыт порт?
        public static bool IsOpen()
        {
            return serialPort.IsOpen;
        }

        // Получить установленную скорость
        public static string GetSpeed()
        {
            if (IsOpen())
                return Convert.ToString(serialPort.BaudRate);

            return "";
        }

        //Получить биты данных
        public static string GetDataBits()
        {
            if (IsOpen())
                return Convert.ToString(serialPort.DataBits);

            return "";
        }

        // Название порта
        public static string GetPortName()
        {
            if (IsOpen())
                return serialPort.PortName;
            
            return "";
        }

        // Стоп-биты
        public static string GetStopBits()
        {
            if(IsOpen())
            {
                switch(serialPort.StopBits) 
                {
                    case StopBits.One: {
                        return "1";
                    }
                    case StopBits.OnePointFive: {
                        return "1.5";
                    }
                    case StopBits.Two: {
                        return "2";
                    }
                    default: return "";
                }
               
            }

            return "";
        }

        // хз
        public static string GetParity()
        {
            if(IsOpen())
            {
                switch (serialPort.Parity)
                {
                    case Parity.None: {
                        return "Нет";
                    }
                    case Parity.Even: {
                        return "Четные";
                    }
                    case Parity.Odd: {
                        return "Нечетные";
                    }
                    default: return "";
                }
            }

            return "";
        }
            
            
    }
}
