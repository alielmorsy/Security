using System;
using System.Globalization;

namespace enc
{
    public class Vigenere
    {
        private string _key;

        public Vigenere(string key)
        {
            this._key = key;
        }

        public string Encrypt(string text)
        {
            string key = generateKey(text);
            string enc = "";
            for (int i = 0; i < text.Length; i++)
            {
                int x = (text[i] + key[i]) % 26;
                enc += (char)(x + 'A');
            }

            return enc;
        }

        public string Decrypt(string text)
        {
            string key = generateKey(text);
            string enc = "";
            for (int i = 0; i < text.Length; i++)
            {
                int x = (text[i] - key[i]+26) % 26;
                enc += (char)(x + 'A');
            }

            return enc;
        }

        private string generateKey(string text)
        {
            string newKey = _key;
            if (newKey.Length > text.Length)
            {
                return newKey.Substring(0, text.Length);
            }

            int currentLength = _key.Length;
            int keyIndex = 0;
            while (currentLength < text.Length)
            {
                newKey += _key[keyIndex++ % _key.Length];
                currentLength++;
            }

            return newKey;
        }
    }
}