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
    class Hamming
    {
        static int[] convert_byte_to_int(byte b)
        {
            if (b > 127)  
                throw new Exception();    
            int[] bMas = new int[7];
            for (int i = 0; i < 7; i++)
                bMas[i] = ((b & (1 << 6 - i)) == 0) ? 0 : 1;
            return bMas;
        }
        public static byte Code(byte b)
        {
            int[] bMas = Hamming.convert_byte_to_int(b);
            bMas[2] = bMas[3];
            bMas[3] = 0;
            int i, c1, c2, c3, coded = 0;             //вычисляем контрольные биты
            c1 = bMas[0] + bMas[2] + bMas[4] + bMas[6];
            c2 = bMas[1] + bMas[2] + bMas[5] + bMas[6];
            c3 = bMas[3] + bMas[4] + bMas[5] + bMas[6];
            bMas[0] = c1 % 2;
            bMas[1] = c2 % 2;
            bMas[3] = c3 % 2;

            for (i = 6; i >= 0; i--)
            {
                coded = coded + bMas[i] * (int)Math.Pow(2, 6 - i);
            }
            return (byte)coded;
        }


        public static byte Decode(byte b)
        {
            int[] bMas = Hamming.convert_byte_to_int(b);
            int[] decMas = new int[4];
            int i, sum = 0;

            decMas[0] = bMas[2];    // в decMas записываю элементы, которые составляют закодированную последовательность хххх
            decMas[1] = bMas[4];
            decMas[2] = bMas[5];
            decMas[3] = bMas[6];
           
            for (i = 3; i >= 0; i--)
            {
                sum = sum + decMas[i] * (int)Math.Pow(2, 3 - i);
            }

            if (b != Hamming.Code((byte)sum)) throw new Exception();

            return (byte)sum;
        }
    }
}
