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
        public static byte[] pack(byte type, byte[] info = null)
        {
            switch (type)
            {
                case 100:
                    {
                        List<byte> L = new List<byte>();
                        L.Add(100);
                        foreach(byte b in info )
                        {
                            L.Add(b);
                        }

                        return L.ToArray();
                        
                    }
            }

            throw new Exception();
        }
        public static void filesAvailableRequest()
        {
            byte[] a = { 2, 4, 8 };
            a = pack(100, a);
            string b = "";
            for (int i = 0; i < a.Length; i++)
            {
                b += Convert.ToString(a[i], 2) + " ";
            }
            MessageBox.Show(b);
        }

    }
}
