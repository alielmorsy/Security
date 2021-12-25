using System;

namespace Security
{
    class Program
    {
        static void Main(string[] args)
        {
            string x = String.Empty;
            byte[] b = new byte[32];
            Console.Write("Key: ");
            foreach (var b1 in b)
            {
                Console.Write(""+b1.ToString("X"));
            }
            Console.WriteLine("\nW: 32");
            Console.WriteLine("R: 12");
            // Array.Fill(b, (byte)1);
            var rc5 = new RC(b, 32, 12);
              string a = rc5.EncryptRC6("A04F5C1DA04F5C1DA04F5C1DA04F5C1D");
              Console.WriteLine("Encrypted: "+a);
            Console.WriteLine(rc5.DecryptRC6(a));
        }
    }
}