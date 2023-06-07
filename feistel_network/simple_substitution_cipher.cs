using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feistel_network
{
    internal class simple_substitution_cipher
    {
        public static byte[] Transform(byte[] data) => data = data.Select(bit => (byte)(255 - bit)).ToArray();
        public static byte[] Restore(byte[] data) => data = data.Select(bit => (byte)(255 - bit)).ToArray();
    }
}
