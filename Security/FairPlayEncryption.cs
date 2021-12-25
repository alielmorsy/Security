using System;
using System.Collections.Generic;

namespace enc
    {
        public class FairPlayEncryption
        {
            private char[,] matrix;

            public FairPlayEncryption(string key)
            {
                CreateMatrix(key);
            }

            private void CreateMatrix(string key)
            {
                key = key.ToLower();
                matrix = new char[5, 5];

                HashSet<char> set = new HashSet<char>();
                foreach (var c in key)
                {
                    if (c == 'j') continue;
                    set.Add(c);
                }

                string temp = key;
                for (int i = 0; i < 26; i++)
                {
                    char a = (char) (i + 97);
                    if (a == 'j') continue;
                    if (!set.Contains(a))
                        temp += a;
                }

                Console.WriteLine(temp);


                int index = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        matrix[i, j] = (char) temp[index++];
            
                    }
            
                }
            }

            public void Encrypt(string text)
            {
                string enc = string.Empty;
                string[] pairs = setupText(text);

                foreach (var pair in pairs)
                {
                    char one = pair[0];
                    char two = pair[1];

                    int pos1X = 0, pos1Y = 0, pos2X = 0, pos2Y = 0;

                    GetPosition(one, ref pos1X, ref pos1Y);
                    GetPosition(two, ref pos2X, ref pos2Y);
                    if (pos1X == pos2X)
                    {
                        pos1Y = (pos1Y + 1) % 5;
                        pos2Y = (pos2Y + 1) % 5;
                    }
                    else if (pos1Y == pos2Y)
                    {
                        pos1X = (pos1X + 1) % 5;
                        pos2X = (pos2X + 1) % 5;
                    }
                    else
                    {
                        int temp = pos1Y;
                        pos1Y = pos2Y;
                        pos2Y = temp;
                    }

                    enc += matrix[pos1X, pos1Y]+string.Empty + matrix[pos2X, pos2Y];
                }

                Console.WriteLine(enc);
            }

            private string[] setupText(string text)
            {
                text = text.Replace('j', 'i');
                string newText = string.Empty;
                bool dummy = true;
                for (int i = 0; i < text.Length; i += 2)
                {
                    if (i + 1 >= text.Length)
                    {
                        
                        dummy = false;
                        newText += text[i];
                        break;
                    }

                    if (text[i] == text[i + 1])
                    {
                        newText += text[i] + "x" + text[i + 1];
                    }
                    else
                    {
                        newText += text[i] + string.Empty + text[i + 1];
                    }
                }

                if (dummy) newText += 'x'; 
                string[] s = new string[newText.Length / 2];

                for (int i = 0, j = 0; i < s.Length; i++, j += 2)
                {
                    s[i] = newText[j] + string.Empty + newText[j + 1];
                    Console.WriteLine(s[i]);
                }

                return s;
            }

            private void GetPosition(char ch, ref int x, ref int y)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        char c = matrix[i, j];
                        if (c == ch)
                        {
                            x = i;
                            y = j;
                        }
                    }
                }
            }
        }
}