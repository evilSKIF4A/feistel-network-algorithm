using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feistel_network
{
    internal class caesars_cipher
    {
        public static byte[] Transform(byte[] data, long key = 3) => data = data.Select(bit => (byte)((bit + key) % 256)).ToArray();
        public static byte[] Restore(byte[] data, long key = 3) => data = data.Select(bit => (byte)((bit - key) % 256)).ToArray();
    }
}
