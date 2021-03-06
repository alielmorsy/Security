using System;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace Security
{
    public class RC
    {
        readonly double GoldenRatio = (1 + Math.Sqrt(5)) / 2;
        private byte[] _key;
        private int _wordLength;
        private int _roundOfRotation;

        public RC(byte[] key, int wordLength, int roundOfRotation)
        {
            this._key = key;
            this._wordLength = wordLength;
            this._roundOfRotation = roundOfRotation;
            InitKey(true);
        }


        public String EncryptRC5(string text)
        {
            uint[] S = InitKey(true);
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

        public string DecryptRC5(string text)
        {
            uint[] S = InitKey(true);
            int move = 2 * (_wordLength / 8);

            uint A = uint.Parse(text.Substring(0, move), NumberStyles.HexNumber);
            uint B = uint.Parse(text.Substring(move, move), NumberStyles.HexNumber);
            for (int i = _roundOfRotation; i >= 1; i--)
            {
                B = (BitOperations.RotateRight(B - S[2 * i + 1], (int)A)) ^ A;
                A = (BitOperations.RotateRight(A - S[2 * i], (int)B)) ^ B;
            }

            B -= S[1];
            A -= S[0];
            return $"{A:X}{B:X}";
        }


        public string EncryptRC6(string text)
        {
            uint[] s = InitKey(false);

            uint[] data = CreateRegisters(text);
            uint A = data[0], B = data[1], C = data[2], D = data[3];

            B += s[0];
            D += s[1];
            uint t, u;
            uint tmp;
            int lgw = (int)Math.Log2(_wordLength);
            for (int i = 1; i <= _roundOfRotation; i++)
            {
                t = BitOperations.RotateLeft(B * (2 * B + 1), lgw);
                u = BitOperations.RotateLeft(D * (2 * D + 1), lgw);
                
                A = BitOperations.RotateLeft(A ^ t, (int)u) + s[2 * i];
                C = BitOperations.RotateLeft(C ^ u, (int)t) + s[2 * i + 1];

                tmp = A;
                A = B;
                B = C;
                C = D;
                D = tmp;
            }

            A += s[2 * _roundOfRotation + 2];
            C += s[2 * _roundOfRotation + 3];
            
            return $"{A:x}{B:x}{C:x}{D:x}".ToUpper();
        }

        public string DecryptRC6(string text)
        {
            uint[] s = InitKey(false);
            uint[] data = CreateRegisters(text);
            uint A = data[0], B = data[1], C = data[2], D = data[3];
            C -= s[2 * _roundOfRotation + 3];
            A -= s[2 * _roundOfRotation + 2];
            uint t, u;
            uint tmp;
            int lgw = (int)Math.Log2(_wordLength);
            for (int i = _roundOfRotation; i >= 1; i--)
            {
                tmp = A;
                A = D;
                D = C;
                C = B;
                B = tmp;
                u = BitOperations.RotateLeft(D * (2 * D + 1), lgw);
                t = BitOperations.RotateLeft(B * (2 * B + 1), lgw);
                C = BitOperations.RotateRight(C - s[2 * i + 1], (int)t) ^ u;
                A = BitOperations.RotateRight(A - s[2 * i], (int)u) ^ t;
            }

            D -= s[1];
            B -= s[0];
        
            return $"{A:x}{B:x}{C:x}{D:x}".ToUpper();
        }

        private uint[] CreateRegisters(string text)
        {
            
            uint[] data = new uint[4];
            data[0] = Convert.ToUInt32(text.Substring(0, 8), 16);
            data[1] = Convert.ToUInt32(text.Substring(8, 8), 16);
            data[2] = Convert.ToUInt32(text.Substring(16, 8), 16);
            data[3] = Convert.ToUInt32(text.Substring(24, 8), 16);
            // int off = 0;
            // for (int i = 0; i < data.Length; i++)
            // {
            //     data[i] = (uint)((input[off++] & 0xff) |
            //                      ((input[off++] & 0xff) << 8) |
            //                      ((input[off++] & 0xff) << 16) |
            //                      ((input[off++] & 0xff) << 24));
            // }

            return data;
        }

        private uint[] InitKey(bool isRc5)
        {
            uint P = Odd((Math.E - 2) * Math.Pow(2, _wordLength));
            uint Q = Odd((GoldenRatio - 1) * Math.Pow(2, _wordLength));

            int u = _wordLength / 8;
            uint[] l = new uint[_key.Length / u];
            int i;
            for (i = _key.Length - 1; i > 0; i--)
            {
                l[i / u] = BitOperations.RotateLeft(l[i / u], 8) + _key[i];
            }

            int t;
            if (isRc5)
            {
                t = (_roundOfRotation + 1) * 2;
            }

            else
            {
                t = _roundOfRotation * 2 + 4;
            }


            uint[] s = new uint[t];
            s[0] = P;
            for (i = 1; i < t; i++)
            {
                s[i] = s[i - 1] + Q;
            }

            //Mixing Keys
            i = 0; //for s array
            int j = 0; // for l array
            uint A = 0;
            uint B = 0; //A for S, B for L
            for (int k = 0; k < 3 * Math.Max(t, l.Length); k++)
            {
                A = s[i] = BitOperations.RotateLeft(s[i] + A + B, 3);
                B = l[j] = BitOperations.RotateLeft(l[j] + A + B, (int)(A + B));
                i = (i + 1) % t;
                j = (j + 1) % l.Length;
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