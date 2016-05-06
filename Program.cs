using System;

namespace CSImpersonator
{
    class Program
    {
        static void Main()
        {
            //using(new 
            Console.WriteLine("Logged in User: " + Environment.UserName);
            using(new CookieJar("user","domain","password"))
            {
                Console.WriteLine("Logged in user: " + Environment.UserName);
                
            }
            Console.WriteLine("Logged in User:" + Environment.UserName);
        }
    }
}
