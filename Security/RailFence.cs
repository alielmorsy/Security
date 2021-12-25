using System;

namespace enc
{
    public class RailFence
    {
        public static string Encrypt(string text, int depth)
        {
            text = text.Replace(" ", "");
            int read = 0;
            string enc = "";
            int smallIndex = 0;
            int index = 0;
            int extra = text.Length % depth;

            int row = text.Length / depth;
            
            while (read<text.Length)
            {
                int row2 = (row*depth)-depth;
                if (extra--> 0)
                {
                    row2 ++;
                }
                if (smallIndex >row2)
                {
                    index++;
                    smallIndex = 0;
                }
                Console.WriteLine((index+smallIndex)+" "+row2);
                enc +=text[ index + smallIndex];
                smallIndex +=  depth ;
                
                read++;
            }

            return enc;
        }

        public static string Decrypt(string text, int depth)
        {
            char[,] rail=new char[depth,text.Length];
            int row = 0;
            int col = 0;
            bool dir_down = false;
            for (int i=0; i < text.Length; i++)
            {
                if (row == 0)
                {
                    dir_down = true;
                }else if (row == depth - 1)
                {
                    dir_down = false;
                }
                rail[row,col++] = '*';
                if (dir_down)
                {
                    row++;
                }
                else row--;
            }
            int index = 0;
            for (int i=0; i<depth; i++)
            for (int j=0; j<text.Length; j++)
                if (rail[i,j] == '*' && index<text.Length)
                    rail[i,j] = text[index++];
            
            string result="";

            row = 0; col = 0;
            for (int i=0; i< text.Length; i++)
            {
                
                if (row == 0)
                    dir_down = true;
                if (row == depth-1)
                    dir_down = false;


                if (rail[row, col] != '*')
                    result += rail[row, col++];
                if (dir_down)
                {
                    row ++ ;
                }
                else
                {
                    row--;
                }
            }
            return result;
        }
    }
}