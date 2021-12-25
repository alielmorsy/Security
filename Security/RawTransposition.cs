using System;
using System.Linq;
using System.Text;

namespace enc
{
    public class RawTransposition
    {
        private string _key;
        private char[,] matrix;
        private int[] _indexs;

        public RawTransposition(string key)
        {
            this._key = key;
        }


        public string Encrypt(string text)
        {
            text = text.Replace(" ", "");
            _handleKey(text);
            int k = 0;
            int col = _key.Length;
            int row = text.Length / col;
            if (text.Length % col != 0)
            {
                row++;
            }

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (k >= text.Length)
                    {
                        matrix[i, j] = '_';
                    }
                    else
                        matrix[i, j] = text[k++];
                }
            }


            int currentIndex = 0;
            String enc = "";

            for (int i = 0; i < col; i++)
            {
                for (k = 0; k < col; k++)
                {
                    if (i == _indexs[k])
                    {
                        break;
                    }
                }


                for (int j = 0; j < row; j++)
                {
                    enc += matrix[j, k];
                    currentIndex++;
                }
            }

            return enc;
        }

        public string Decryption(string text)
        {
            int col = _key.Length;
            int row = text.Length / col;
            if (text.Length % col != 0)
            {
                row++;
            }

            _handleKey(text);
            for (int i = 0; i < col; i++)
            {
                int j;
                for (j = 0; j < col; j++)
                {
                    if (i == _indexs[j])
                    {
                        break;
                    }
                }

                Console.Write(j + " ");
                int index = 0;
                for (int k = 0; k < row; k++)
                {
                    //Console.Write(text[(j*col) + index++]);
                    matrix[k, j] = text[(i * col) + index++];
                }
            }

            Console.WriteLine();
            string dec = "";
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (matrix[i, j] == '_') continue;
                    dec += matrix[i, j];
                }
            }

            return dec;
        }

        private void _handleKey(string text)
        {
            int col = _key.Length;
            int row = text.Length / col;
            if (text.Length % col != 0)
            {
                row++;
            }

            _indexs = new int[_key.Length];
            matrix = new char[row, col];

            string sortedKey = new string(_key.Distinct().OrderBy(c => c).ToArray());
            for (int i = 0; i < _indexs.Length; i++)
            {
                _indexs[i] = sortedKey.IndexOf(_key[i]);
            }
        }
    }
}