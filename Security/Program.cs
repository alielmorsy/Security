using System;

namespace Security
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] b = new byte[32];
           // Array.Fill(b, (byte)1);
            var rc5 = new RC5(b, 32,5);
            rc5.Encrypt("A04F5C1DA04F5C1D");
        }
    }
}