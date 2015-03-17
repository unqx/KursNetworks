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
            catch(System.UnauthorizedAccessException e)
            {
                const string message = "Доступ к COM-порту закрыт!";
                const string caption = "Ошибка";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
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
            else
                return "";
        }

        public static string GetDataBits()
        {
            if (IsOpen())
                return Convert.ToString(serialPort.DataBits);
            else
                return "";
        }
    }
}
