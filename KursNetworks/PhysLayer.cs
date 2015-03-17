using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;

namespace KursNetworks
{
    class PhysLayer
    {
        private static SerialPort serialPort = new SerialPort();

        // Скан портов
        public static string[] scanPorts()
        {
            return SerialPort.GetPortNames();
        }

        public static void EstablishConnection(string name, int rate, int dataBits, StopBits S, Parity P)
        {
            serialPort.PortName = name;
            serialPort.BaudRate = rate;
            serialPort.DataBits = dataBits;
            serialPort.StopBits = S;
            serialPort.Parity = P;

            try
            {
                serialPort.Open();
            }
            catch(System.UnauthorizedAccessException)
            {
                const string message = "Доступ к COM-порту закрыт!";
                const string caption = "Ошибка";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        public static void DropConnection()
        {
            serialPort.Close();
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

        public static string GetPortName()
        {
            if (IsOpen())
                return serialPort.PortName;
            
            return "";
        }

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
