namespace enc
{
    public class Caesar
    {
        public static string Encrypt(string text, int n)
        {
            string enc = "";
            for (int i = 0; i < text.Length; i++)
            {
                int x = (text[i] +n - 65) % 26;
                x += 65;

                enc += (char) x;
            }

            return enc;
        }  public static string Decrypt(string text, int n)
        {
            string enc = "";
            for (int i = 0; i < text.Length; i++)
            {
                int x = (text[i] -n - 65) % 26;
                x += 65;

                enc += (char) x;
            }

            return enc;
        }
    }
}