using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using System.IO;
using System.Collections.Concurrent;

namespace KursNetworks
{
    class DataLink
    {
        // Флаги и свойства
        public static string[] files = null;
        public static bool filesUpdated = false;
        public static bool Connection = false;

        public static ConcurrentQueue<File> SendQueue = new ConcurrentQueue<File>();


        public static bool FileSending = false;
        public static bool FileRecieving = false;
        public static string FileRecievingName = "";
        public static int FileRecievingSize = 0;


        public static byte[] pack(char type, byte[] info = null)
        {
            switch (type)
            {
                case 'I':
                case 'Z':
                case 'X':
                    {
                        List<byte> L = new List<byte>();
                        L.Add(Convert.ToByte(type));
                        if (info != null)
                        {
                            foreach (byte b in info)
                            {
                                L.Add(b);
                            }
                        }
                        return L.ToArray();
                    }

                case 'F':
                case 'D':
                case 'E':
                case 'S':
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
                case 'A':
                case 'N':
                    {
                        byte[] arr = { Convert.ToByte(type) };
                        return arr;
                    }
            }

            throw new Exception();
        }

        // Кодирование кадра
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

        // Раскодирование кадра
        public static byte[] DecodeFrame(byte[] frame)
        {
            List<byte> L = new List<byte>();
            for (int i = 0; i < frame.Length; i += 2)
            {
                byte decoded = Convert.ToByte(Hamming.Decode(frame[i]) << 4);
                decoded = Convert.ToByte(decoded ^ Hamming.Decode(frame[i+1]));
                L.Add(decoded);
            }

            return L.ToArray();
        }

        // Определяем пакет
        public static void Analyze(byte[] recievedArray)
        {
            try
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

                if (recievedArray[0] == Convert.ToByte('D'))
                {
                    if (recievedArray[1] == Constants.BORDER)
                    {
                        List<byte> FileNameBytes = new List<byte>();
                        for (int i = 2; i < recievedArray.Length; i++)
                        {
                            if (recievedArray[i] == Constants.BORDER)
                                break;
                            FileNameBytes.Add(recievedArray[i]);
                        }

                        string FileName = Encoding.Default.GetString(FileNameBytes.ToArray());

                        try
                        {
                            string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            string fullPath = Desktop + "\\" + FileName;

                            if(System.IO.File.Exists(fullPath))
                            {
                                FileInfo I = new FileInfo(fullPath);
                                File F = new File(fullPath, I.Length);
                                SendQueue.Enqueue(F);
                            }

                            else
                            {
                                FileNotFound();
                            }
                            

                        }
                        catch(Exception)
                        { 
                           
                        }
                    }
                }

                if (recievedArray[0] == Convert.ToByte('S'))
                {
                    if (recievedArray[1] == Constants.BORDER)
                    {
                        List<byte> SizeBytes = new List<byte>();
                        for (int i = 2; i < recievedArray.Length; i++)
                        {
                            if (recievedArray[i] == Constants.BORDER)
                                break;
                            SizeBytes.Add(recievedArray[i]);
                        }

                        string Size = Encoding.Default.GetString(SizeBytes.ToArray());
                        DataLink.FileRecievingSize = Convert.ToInt32(Size);
                        DataLink.FileRecieving = true;
                        ACK();
                    }
                }

                // Квитанции

                if(recievedArray[0] == Convert.ToByte('A'))
                {
                    PhysLayer.Responses.Enqueue(recievedArray[0]);
                }

                // Инфокадр!
                if(recievedArray[0] == Convert.ToByte('I'))
                {
                    List<byte> InfoBytes = new List<byte>();
                    for (int i = 1; i < recievedArray.Length; i++)
                    {
                        InfoBytes.Add(recievedArray[i]);
                    }
                    PhysLayer.FramesRecieved.Enqueue(InfoBytes.ToArray());
                    ACK();
                }

                if (recievedArray[0] == Convert.ToByte('Z') || recievedArray[0] == Convert.ToByte('X'))
                {
                    List<byte> InfoBytes = new List<byte>();

                    for (int i = 1; i < recievedArray.Length; i++)
                    {
                        InfoBytes.Add(recievedArray[i]);
                    }
                    PhysLayer.FramesRecieved.Enqueue(InfoBytes.ToArray());
                }
            }
            catch(Exception)
            {
                if (DataLink.FileRecieving)
                {
                    NAK();
                }
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

        public static void DownloadRequest(string FileName)
        {
            List<byte> f = new List<byte>();
            // делаем массив байтов для названия файла
            byte[] fBytes = Encoding.Default.GetBytes(FileName);

            foreach (byte b in fBytes)
            {
                f.Add(b);
            }

            byte[] a = pack('D', f.ToArray());
            a = DataLink.EncodeFrame(a);
            PhysLayer.Write(a);
        }

        public static void StartSendingFile(File F)
        {
            List<byte> f = new List<byte>();

            byte[] fBytes = Encoding.Default.GetBytes(Convert.ToString(F.Size));

            foreach (byte b in fBytes)
            {
                f.Add(b);
            }

            byte[] a = pack('S', f.ToArray());
            a = DataLink.EncodeFrame(a);
            PhysLayer.Write(a);

        }

        public static void FileNotFound()
        {
            byte[] fnf = Encoding.Default.GetBytes("FNF");
            fnf = pack('X', fnf);
            fnf = EncodeFrame(fnf);
            PhysLayer.Write(fnf);
        }

        public static void ACK()
        {
            byte[] a = pack('A');
            a = DataLink.EncodeFrame(a);
            PhysLayer.Write(a);
        }

        public static void NAK()
        {
            byte[] a = pack('N');
            a = DataLink.EncodeFrame(a);
            PhysLayer.Write(a);
        }

        public static void EOF()
        {
            byte[] eof = Encoding.Default.GetBytes("EOF");
            eof = pack('Z', eof);
            eof = EncodeFrame(eof);
            PhysLayer.Write(eof);
        }



    }
}
