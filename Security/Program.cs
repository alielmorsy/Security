using System;

namespace Security
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] b = new byte[32];
            // Array.Fill(b, (byte)1);
            var rc5 = new RC(b, 32, 20);
            string a = rc5.EncryptRC5("A04F5C1DA04F5C1D");
            Console.WriteLine(a);
            Console.WriteLine(rc5.DecryptRC5(a));
        }
    }
}