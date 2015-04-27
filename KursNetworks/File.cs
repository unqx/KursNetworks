using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursNetworks
{
    class File
    {
        public string Name { get; set; }

        public long Size { get; set; }

        public File(string n, long s)
        {
            Name = n;
            Size = s;
        }
    }
}
