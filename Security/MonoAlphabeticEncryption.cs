using System;
using System.Linq;

namespace enc
{
    public class MonoAlphabeticEncryption
    {
        private static char[] _chars =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
            'v', 'w', 'x', 'y', 'z'
        };

        private static char[] shifted;

        public static string Encrypt(string text)
        {
            string enc = String.Empty;

            if (shifted == null)
            {
                Random random = new Random();
                shifted = _chars.OrderBy(c => random.Next()).ToArray();
                foreach (var c in shifted)
                {
                    Console.Write(c);
                }
                Console.WriteLine();
            }

            foreach (var c in text)
            {
                int index = GetIndex(c, _chars);
                enc += shifted[index];
            }

            return enc;
        }

        private static int GetIndex(char c, char[] chars)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                if (c == chars[i])
                {
                    return i;
                }
            }

            return -1;
        }
    }
}