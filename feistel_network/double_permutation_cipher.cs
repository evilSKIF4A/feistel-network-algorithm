using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feistel_network
{
    internal class double_permutation_cipher
    {
        public static byte[] Transform(byte[] data, string key_horizonal = "54321", string key_vertical = "12345")
        {
            List<List<byte>> block = new List<List<byte>>();

            List<int> char_key_h = key_horizonal.ToCharArray().Select(ch => (int)ch).ToList();
            List<int> char_key_v = key_vertical.ToCharArray().Select(ch => (int)ch).ToList();

            for (int i = key_horizonal.Length; i < data.Length; i += key_horizonal.Length)
            {
                block.Add(data.Take(i).Skip(i - key_horizonal.Length).ToList());
                if (i + key_horizonal.Length >= data.Length)
                {
                    List<byte> temp = data.Skip(i).ToList();
                    while (temp.Count != key_horizonal.Length)
                        temp.Add(255);
                    block.Add(temp);
                }
            }

            byte[][] permutation_horizontal = new byte[block.Count][];
            for (int i = 0; i < permutation_horizontal.Length; i++)
                permutation_horizontal[i] = new byte[key_horizonal.Length];
            int j = 0;
            while (j != key_horizonal.Length)
            {
                int k = char_key_h.Select((n, i) => new { Value = n, Index = i }).Aggregate((a, b) => a.Value < b.Value ? a : b).Index;
                char_key_h[k] = 99999999;
                for (int i = 0; i < block.Count; i++)
                    permutation_horizontal[i][j] = block[i][k];
                j++;
            }

            List<byte[]> permutation_vertical = new List<byte[]>();
            for (int h = key_vertical.Length; h < permutation_horizontal.Length; h += key_vertical.Length)
            {
                List<byte[]> temp;

                temp = permutation_horizontal.Take(h).Skip(h - key_vertical.Length).Select(x => (byte[])x.Clone()).ToList();
                List<int> temp_char_key_v = char_key_v.ToList();

                int rev = 0;
                while (rev != key_vertical.Length)
                {
                    int k = temp_char_key_v.Select((n, i) => new { Value = n, Index = i }).Aggregate((a, b) => a.Value < b.Value ? a : b).Index;
                    temp_char_key_v[k] = 99999999;
                    byte[] temp_arr = temp[rev].ToArray();
                    temp[rev] = temp[k].ToArray();
                    temp[k] = temp_arr;
                    rev++;
                }
                foreach (byte[] arr in temp)
                    permutation_vertical.Add((byte[])arr.Clone());

                if (h + key_vertical.Length >= permutation_horizontal.Length)
                {
                    temp = permutation_horizontal.Skip(h).Select(x => (byte[])x.Clone()).ToList();
                    temp = temp.Select(x => x.Select(y => y = y != 0 ? y : (byte)255).ToArray()).ToList();
                    while(temp.Count != key_vertical.Length)
                    {
                        temp.Add(new byte[key_horizonal.Length].Select(x => x = 255).ToArray());
                    }

                    rev = 0;
                    while (rev != key_vertical.Length)
                    {
                        int k = temp_char_key_v.Select((n, i) => new { Value = n, Index = i }).Aggregate((a, b) => a.Value < b.Value ? a : b).Index;
                        temp_char_key_v[k] = 99999999;
                        byte[] temp_arr = temp[rev].ToArray();
                        temp[rev] = temp[k].ToArray();
                        temp[k] = temp_arr;
                        rev++;
                    }
                    foreach (byte[] arr in temp)
                        permutation_vertical.Add((byte[])arr.Clone());
                }
            }

            List<byte> new_data = new List<byte>();
            foreach (byte[] list in permutation_vertical)
                foreach (byte bit in list)
                        new_data.Add(bit);

            return new_data.ToArray();
        }

        public static byte[] Restore(byte[] data, string key_horizonal = "54321", string key_vertical = "12345")
        {
            List<List<byte>> block = new List<List<byte>>();

            List<int> char_key_h = key_horizonal.ToCharArray().Select(ch => (int)ch).ToList();
            List<int> char_key_v = key_vertical.ToCharArray().Select(ch => (int)ch).ToList();

            for (int i = key_horizonal.Length; i < data.Length; i += key_horizonal.Length)
            {
                block.Add(data.Take(i).Skip(i - key_horizonal.Length).ToList());
                if (i + key_horizonal.Length >= data.Length)
                {
                    List<byte> temp = data.Skip(i).ToList();
                    while (temp.Count != key_horizonal.Length)
                        temp.Add(255);
                    block.Add(temp);
                }
            }

            List<byte[]> new_block = new List<byte[]>();

            for (int h = key_vertical.Length; h < block.Count; h += key_vertical.Length)
            {
                List<byte[]> temp;

                temp = block.Take(h).Skip(h - key_vertical.Length).Select(x => x.ToArray()).ToList();
                List<int> temp_char_key_v = char_key_v.ToList();

                int rev = 0;
                while (rev != key_vertical.Length)
                {
                    int k = temp_char_key_v.Select((n, i) => new { Value = n, Index = i }).Aggregate((a, b) => a.Value < b.Value ? a : b).Index;
                    temp_char_key_v[k] = 99999999;
                    byte[] temp_arr = temp[rev].ToArray();
                    temp[rev] = temp[k].ToArray();
                    temp[k] = temp_arr;
                    rev++;
                }
                foreach (byte[] arr in temp)
                    new_block.Add((byte[])arr.Clone());

                if (h + key_vertical.Length >= block.Count)
                {
                    temp = block.Skip(h).Select(x => x.ToArray()).ToList();
                    temp = temp.Select(x => x.Select(y => y = y != 0 ? y : (byte)255).ToArray()).ToList();
                    while (temp.Count != key_vertical.Length)
                    {
                        temp.Add(new byte[key_horizonal.Length].Select(x => x = 255).ToArray());
                    }

                    rev = 0;
                    while (rev != key_vertical.Length)
                    {
                        int k = temp_char_key_v.Select((n, i) => new { Value = n, Index = i }).Aggregate((a, b) => a.Value < b.Value ? a : b).Index;
                        temp_char_key_v[k] = 99999999;
                        byte[] temp_arr = temp[rev].ToArray();
                        temp[rev] = temp[k].ToArray();
                        temp[k] = temp_arr;
                        rev++;
                    }
                    foreach (byte[] arr in temp)
                        new_block.Add((byte[])arr.Clone());
                }
            }

            byte[][] permutation_horizontal = new byte[new_block.Count][];
            for (int i = 0; i < permutation_horizontal.Length; i++)
                permutation_horizontal[i] = new byte[key_horizonal.Length];

            int j = 0;
            while (j != key_horizonal.Length)
            {
                int k = char_key_h.Select((n, i) => new { Value = n, Index = i }).Aggregate((a, b) => a.Value < b.Value ? a : b).Index;
                char_key_h[k] = 99999999;

                for (int i = 0; i < block.Count; i++)
                    permutation_horizontal[i][k] = new_block[i][j];
                j++;
            }

            List<byte> new_data = new List<byte>();
            foreach (byte[] list in permutation_horizontal)
                foreach (byte bit in list)
                    if (bit != 255)
                        new_data.Add(bit);

            return new_data.ToArray();
        }
    }
}
