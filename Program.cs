using System;

namespace CSImpersonator
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Logged in User: " + Environment.UserName);
            using (new CookieJar("user", "domain", "password"))
            {
                Console.WriteLine("Logged in user: " + Environment.UserName);
            }
            Console.WriteLine("Logged in User:" + Environment.UserName);
        }
    }
}