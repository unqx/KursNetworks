using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using System.IO;

namespace KursNetworks
{
    class DataLink
    {
        public static string[] files = null;
        public static bool filesUpdated = false;

        public static bool Connection = false;

        public static byte[] pack(char type, byte[] info = null)
        {
            switch (type)
            {
                case 'F':
                    {
                        List<byte> L = new List<byte>();
                        L.Add(Convert.ToByte(type));
                        L.Add(Constants.BORDER);
                        if (info != null)
                        {
                            foreach (byte b in info)
                            {
                                L.Add(b);
                            }
                        }
                       
                        L.Add(Constants.BORDER);
                        return L.ToArray();
                    }

                case 'R':
                    {
                        byte[] arr = { Convert.ToByte(type) };
                        return arr;
                    }

                case 'E':
                    {
                        List<byte> L = new List<byte>();
                        L.Add(Convert.ToByte(type));
                        L.Add(Constants.BORDER);
                        if (info != null)
                        {
                            foreach (byte b in info)
                            {
                                L.Add(b);
                            }
                        }

                        L.Add(Constants.BORDER);
                        return L.ToArray();
                    }
            }

            throw new Exception();
        }

        public static byte[] EncodeFrame(byte[] frame)
        {
            List<byte> L = new List<byte>();

            foreach (byte b in frame)
            {
                var right = Convert.ToByte(b & 0x0F);
                var left = Convert.ToByte(b >> 4);
                
                L.Add(Hamming.Code(left));
                L.Add(Hamming.Code(right));
            }

            return L.ToArray();
        }

        public static byte[] DecodeFrame(byte[] frame)
        {
            List<byte> L = new List<byte>();

            try
            {
                for (int i = 0; i < frame.Length; i += 2)
                {
                    byte decoded = Convert.ToByte(Hamming.Decode(frame[i]) << 4);
                    decoded = Convert.ToByte(decoded ^ Hamming.Decode(frame[i+1]));
                    L.Add(decoded);
                }
            }
            catch(Exception)
            {
                MessageBox.Show("XEROVO");
            }

            return L.ToArray();
        }

        // Определяем пакет
        public static void Analyze(byte[] recievedArray)
        {
            recievedArray = DataLink.DecodeFrame(recievedArray);
            if (recievedArray[0] == Convert.ToByte('F'))
            {
                if (recievedArray[1] == Constants.BORDER)
                {
                    List<byte> filesList = new List<byte>();
                    for (int i = 2; i < recievedArray.Length; i++)
                    {
                        if (recievedArray[i] == Constants.BORDER)
                            break;
                        filesList.Add(recievedArray[i]);
                    }

                    string fileString = Encoding.Default.GetString(filesList.ToArray());
                    DataLink.files = fileString.Split('-');
                    DataLink.filesUpdated = true;
                }
            }

            if (recievedArray[0] == Convert.ToByte('E'))
            {
                if (recievedArray[1] == Constants.BORDER)
                {
                    List<byte> PortNameBytes = new List<byte>();
                    for (int i = 2; i < recievedArray.Length; i++)
                    {
                        if (recievedArray[i] == Constants.BORDER)
                            break;
                        PortNameBytes.Add(recievedArray[i]);
                    }

                    string PortName = Encoding.Default.GetString(PortNameBytes.ToArray());
                    PhysLayer.PortReciever = PortName;
                }
            }

            if (recievedArray[0] == Convert.ToByte('R'))
            {
                DataLink.SendAvailableFiles();
            }
        }

        // Запрос файлов
        public static void RequestAvailableFiles()
        {
            byte[] a = pack('R');
            a = DataLink.EncodeFrame(a);
            PhysLayer.Write(a);
        }

        // Отправка доступных файлов
        public static void SendAvailableFiles()
        {
            // список для имен файлов по байтам
            List<byte> f = new List<byte>();

            // достаем все файлы с рабочего стола
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); 
            string[] files = Directory.GetFiles(desktop);

            foreach (string str in files)
            {
                // делаем массив байтов для каждого файла
                byte[] fBytes = Encoding.Default.GetBytes(Path.GetFileName(str) + "-");

                //побайтово кладем в массив
                foreach (byte b in fBytes)
                {
                    f.Add(b);
                }
            }
            
            byte[] a = pack('F', f.ToArray());
            a = DataLink.EncodeFrame(a);
            PhysLayer.Write(a);

        }

        // Кадр для установления логического соединения с названием порта
        public static void EstablishConnection()
        {
            List<byte> f = new List<byte>();

            // делаем массив байтов для имени порта
            byte[] fBytes = Encoding.Default.GetBytes(PhysLayer.GetPortName());

            //побайтово кладем в массив
            foreach (byte b in fBytes)
            {
                f.Add(b);
            }

            byte[] a = pack('E', f.ToArray());
            a = DataLink.EncodeFrame(a);
            PhysLayer.Write(a);
        }

    }
}
