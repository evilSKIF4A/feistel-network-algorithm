using System;
using System.IO;
using System.Linq;

namespace feistel_network
{
    internal class Program
    {
        static void Main(string[] args)
        {
            byte[] data = File.ReadAllBytes("data_long.txt");

            int rounds = 4;
            int[] keys_caesars = new int[] { 300, 10, 4433, 123 };
            string[] keys_simple_permutation = new string[] { "fsodfoa", "234ewfwf", "j249fjdsf", "382jfwejfj" };

            string[] keys_double_permutation_hor = new string[] { "8fh3hf8", "389hefjpfe", "2h8fhdh", "g42j0jqfsdj" };
            string[] keys_double_permutation_ver = new string[] { "j0wj0q3", "f320j0ew0fj", "2309j0ejf", "f3280j0ef" };
            File.WriteAllBytes("encript_data_long.bin", double_permutation_fn_crypt(simple_permutation_fn_crypt(caesars_fn_crypt(simple_substitution_fn_crypt(data, rounds), rounds, keys_caesars), rounds, keys_simple_permutation), rounds, keys_double_permutation_hor, keys_double_permutation_ver));

            File.WriteAllBytes("decript_data_long.txt", simple_substitution_fn_decrypt(caesars_fn_decrypt(simple_permutation_fn_decrypt(double_permutation_fn_decrypt(File.ReadAllBytes("encript_data_long.bin"), rounds, keys_double_permutation_hor, keys_double_permutation_ver), rounds, keys_simple_permutation), rounds, keys_caesars), rounds));

        }

        private static byte[] simple_substitution_fn_crypt(byte[] data, int rounds)
        {
            int mid = data.Length / 2;
            byte[] L = data.Take(mid).ToArray();
            byte[] R = data.Skip(mid).ToArray();
            if (R.Length != L.Length)
                L = L.Concat(new byte[] { 0 }).ToArray();
            for (int i = 0; i < rounds; i++)
            {
                byte[] temp = new byte[R.Length];
                byte[] F = simple_substitution_cipher.Transform(L);
                for (int j = 0; j < R.Length; j++)
                    temp[j] = (byte)(R[j] ^ F[j]);
                R = L;
                L = temp;
            }
            return L.Concat(R).ToArray();
        }

        private static byte[] simple_substitution_fn_decrypt(byte[] data, int rounds)
        {
            int mid = data.Length / 2;
            byte[] L = data.Take(mid).ToArray();
            byte[] R = data.Skip(mid).ToArray();
            if (R.Length != L.Length)
                L = L.Concat(new byte[] { 0 }).ToArray();
            for (int i = 0; i < rounds; i++)
            {
                byte[] temp = new byte[L.Length];
                byte[] F = simple_substitution_cipher.Transform(R);
                for (int j = 0; j < L.Length; j++)
                    temp[j] = (byte)(L[j] ^ F[j]);
                L = R;
                R = temp;
            }
            return L.Concat(R).ToArray();
        }
    
        private static byte[] caesars_fn_crypt(byte[] data, int rounds, int[] keys)
        {
            int mid = data.Length / 2;
            byte[] L = data.Take(mid).ToArray();
            byte[] R = data.Skip(mid).ToArray();
            if (R.Length != L.Length)
                L = L.Concat(new byte[] { 0 }).ToArray();
            for (int i = 0; i < rounds; i++)
            {
                byte[] temp = new byte[R.Length];
                byte[] F = caesars_cipher.Transform(L, keys[i]);
                for (int j = 0; j < R.Length; j++)
                    temp[j] = (byte)(R[j] ^ F[j]);
                R = L;
                L = temp;
            }
            return L.Concat(R).ToArray();
        }

        private static byte[] caesars_fn_decrypt(byte[] data, int rounds, int[] keys)
        {
            int mid = data.Length / 2;
            byte[] L = data.Take(mid).ToArray();
            byte[] R = data.Skip(mid).ToArray();
            if (R.Length != L.Length)
                L = L.Concat(new byte[] { 0 }).ToArray();
            for (int i = rounds - 1; i >= 0; i--)
            {
                byte[] temp = new byte[L.Length];
                byte[] F = caesars_cipher.Transform(R, keys[i]);
                for (int j = 0; j < L.Length; j++)
                    temp[j] = (byte)(L[j] ^ F[j]);
                L = R;
                R = temp;
            }
            return L.Concat(R).ToArray();
        }

        private static byte[] simple_permutation_fn_crypt(byte[] data, int rounds, string[] keys)
        {
            int mid = data.Length / 2;
            byte[] L = data.Take(mid).ToArray();
            byte[] R = data.Skip(mid).ToArray();
            if (R.Length != L.Length)
                L = L.Concat(new byte[] { 0 }).ToArray();
            for (int i = 0; i < rounds; i++)
            {
                byte[] temp = new byte[R.Length];
                byte[] F = simple_permutation_encryption.Transform(L, keys[i]);
                for (int j = 0; j < R.Length; j++)
                    temp[j] = (byte)(R[j] ^ F[j]);
                R = L;
                L = temp;
            }
            return L.Concat(R).ToArray();
        }

        private static byte[] simple_permutation_fn_decrypt(byte[] data, int rounds, string[] keys)
        {
            int mid = data.Length / 2;
            byte[] L = data.Take(mid).ToArray();
            byte[] R = data.Skip(mid).ToArray();
            if (R.Length != L.Length)
                L = L.Concat(new byte[] { 0 }).ToArray();
            for (int i = rounds - 1; i >= 0; i--)
            {
                byte[] temp = new byte[L.Length];
                byte[] F = simple_permutation_encryption.Transform(R, keys[i]);
                for (int j = 0; j < L.Length; j++)
                    temp[j] = (byte)(L[j] ^ F[j]);
                L = R;
                R = temp;
            }
            return L.Concat(R).ToArray();
        }

        private static byte[] double_permutation_fn_crypt(byte[] data, int rounds, string[] keys_hor, string[] keys_ver)
        {
            int mid = data.Length / 2;
            byte[] L = data.Take(mid).ToArray();
            byte[] R = data.Skip(mid).ToArray();
            if (R.Length != L.Length)
                L = L.Concat(new byte[] { 0 }).ToArray();
            for (int i = 0; i < rounds; i++)
            {
                byte[] temp = new byte[R.Length];
                byte[] F = double_permutation_cipher.Transform(L, keys_hor[i], keys_ver[i]);
                for (int j = 0; j < R.Length; j++)
                    temp[j] = (byte)(R[j] ^ F[j]);
                R = L;
                L = temp;
            }
            return L.Concat(R).ToArray();
        }

        private static byte[] double_permutation_fn_decrypt(byte[] data, int rounds, string[] keys_hor, string[] keys_ver)
        {
            int mid = data.Length / 2;
            byte[] L = data.Take(mid).ToArray();
            byte[] R = data.Skip(mid).ToArray();
            if (R.Length != L.Length)
                L = L.Concat(new byte[] { 0 }).ToArray();
            for (int i = rounds - 1; i >= 0; i--)
            {
                byte[] temp = new byte[L.Length];
                byte[] F = double_permutation_cipher.Transform(R, keys_hor[i], keys_ver[i]);
                for (int j = 0; j < L.Length; j++)
                    temp[j] = (byte)(L[j] ^ F[j]);
                L = R;
                R = temp;
            }
            return L.Concat(R).ToArray();
        }
    }
}
