using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feistel_network
{
    internal class simple_permutation_encryption
    {
        public static byte[] Transform(byte[] data, string key_horizontal = "54321")
        {
            List<List<byte>> block = new List<List<byte>>();

            List<int> number_sym_alphavit = key_horizontal.ToCharArray().Select(ch => (int)ch).ToList();
            for (int i = key_horizontal.Length; i < data.Length; i += key_horizontal.Length)
            {
                block.Add(data.Take(i).Skip(i - key_horizontal.Length).ToList());
                if(i + key_horizontal.Length >= data.Length)
                {
                    List<byte> temp = data.Skip(i).ToList();
                    while (temp.Count != key_horizontal.Length)
                        temp.Add(1);
                    block.Add(temp);
                }
            }

            byte[][] permutation = new byte[block.Count][];
            for (int i = 0; i < permutation.Length; i++)
                permutation[i] = new byte[key_horizontal.Length];
            int j = 0;
            while (j != key_horizontal.Length)
            {
                int k = number_sym_alphavit.Select((n, i) => new { Value = n, Index = i }).Aggregate((a, b) => a.Value < b.Value ? a : b).Index;
                number_sym_alphavit[k] = 99999999;
                for (int i = 0; i < block.Count; i++)
                    permutation[i][j] = block[i][k];
                j++;
            }

            List<byte> new_data = new List<byte>();
            foreach (byte[] list in permutation)
                foreach (byte bit in list)
                    //if (bit != 1)
                        new_data.Add(bit);

            return new_data.ToArray();
        }

        public static byte[] Restore(byte[] data, string key_horizontal = "54321")
        {
            List<List<byte>> block = new List<List<byte>>();

            List<int> number_sym_alphavit = key_horizontal.ToCharArray().Select(ch => (int)ch).ToList();
            for (int i = key_horizontal.Length; i < data.Length; i += key_horizontal.Length)
            {
                block.Add(data.Take(i).Skip(i - key_horizontal.Length).ToList());
                if(i + key_horizontal.Length >= data.Length)
                {
                    List<byte> temp = data.Skip(i).ToList();
                    while (temp.Count != key_horizontal.Length)
                        temp.Add(255);
                    block.Add(temp);
                }
            }

            byte[][] permutation = new byte[block.Count][];
            for (int i = 0; i < permutation.Length; i++)
                permutation[i] = new byte[key_horizontal.Length];
            int j = 0;
            while (j != key_horizontal.Length)
            {
                int k = number_sym_alphavit.Select((n, i) => new { Value = n, Index = i }).Aggregate((a, b) => a.Value < b.Value ? a : b).Index;
                number_sym_alphavit[k] = 99999999;
                for (int i = 0; i < block.Count; i++)
                    permutation[i][k] = block[i][j];
                j++;
            }

            List<byte> new_data = new List<byte>();
            foreach (byte[] list in permutation)
                foreach (byte bit in list)
                    if(bit != 255)
                        new_data.Add(bit);

            return new_data.ToArray();
        }
    }
}
