using System;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Security
{
    public class RC5
    {
        readonly double GoldenRatio = (1 + Math.Sqrt(5)) / 2;
        private byte[] _key;
        private int _wordLength;
        private int _roundOfRotation;

        public RC5(byte[] key, int wordLength, int roundOfRotation)
        {
            this._key = key;
            this._wordLength = wordLength;
            this._roundOfRotation = roundOfRotation;
            uint[] b = initKey();
            foreach (var VARIABLE in b)
            {
                Console.Write(VARIABLE.ToString("X") + " ");
            }
        }


        public String Encrypt(string text)
        {
            uint[] S = initKey();
            int move = 2 * (_wordLength / 8);

            uint A = uint.Parse(text.Substring(0, move), NumberStyles.HexNumber) + S[0];
            uint B = uint.Parse(text.Substring(move, move), NumberStyles.HexNumber) + S[1];
            for (int i = 1; i <= _roundOfRotation; i++)
            {
                A = BitOperations.RotateLeft(A ^ B, (int)B) + S[2 * i];
                B = BitOperations.RotateLeft(A ^ B, (int)A) + S[2 * i + 1];
            }

            
            return $"{A:X}{B:X}";
        }

        private uint[] initKey()
        {
            uint P = Odd((Math.E - 2) * Math.Pow(2, _wordLength));
            uint Q = Odd((GoldenRatio - 1) * Math.Pow(2, _wordLength));

            Console.WriteLine("P: " + P.ToString("X"));
            Console.WriteLine("Q: " + Q.ToString("X"));
            int u = _wordLength / 8;
            uint[] l = new uint[_key.Length / u];
            int i;
            for (i = _key.Length - 1; i > 0; i--)
            {
                l[i / u] = BitOperations.RotateLeft(l[u / i], 8) + _key[i];
            }

            int t = (_roundOfRotation + 1) * 2;
            uint[] s = new uint[t];
            s[0] = P;
            for (i = 1; i < t; i++)
            {
                s[i] = s[i - 1] + Q;
            }

            //Mixing Keys
            i = 0; //for l array
            int j = 0; // for s array
            uint A = 0;
            uint B = 0; //A for S, B for L
            for (int k = 0; k < 3 * Math.Max((uint)t, l.Length); k++)
            {
                A = s[j] = BitOperations.RotateLeft(s[i] + A + B, 3);
                B = l[i] = BitOperations.RotateLeft(l[i] + A + B, (int)(A + B));
                i = (i + i) % l.Length;
                j = (j + 1) % t;
            }

            return s;
        }

        public static uint Odd(double number)
        {
            uint n = (uint)Math.Round(number);

            return n % 2 == 0 ? n + 1 : n;
        }
    }
}