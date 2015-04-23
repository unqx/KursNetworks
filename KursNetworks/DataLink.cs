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
            }

            throw new Exception();
        }

        public static void Analyze(byte[] recievedArray)
        {
            if (recievedArray[0] == Convert.ToByte('F'))
            {
                if (recievedArray[1] == Constants.BORDER)
                {
                    List<byte> filesList = new List<byte>();
                    for (int i = 2; recievedArray[i] != Constants.BORDER; i++)
                    {
                        filesList.Add(recievedArray[i]);
                    }

                    string fileString = Encoding.Default.GetString(filesList.ToArray());
                    DataLink.files = fileString.Split('-');
                    DataLink.filesUpdated = true;
                }
            }

            if (recievedArray[0] == Convert.ToByte('R'))
            {
                DataLink.SendAvailableFiles();
            }
        }

        public static void RequestAvailableFiles()
        {
            byte[] a = pack('R');
            PhysLayer.Write(a);
        }

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
            PhysLayer.Write(a);

        }

    }
}
