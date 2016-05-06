using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTools;

namespace CSImpersonator
{
    class Program
    {
        static void Main(string[] args)
        {
            //using(new 
            Console.WriteLine("Logged in User: " + Environment.UserName.ToString());
            using(new CookieJar("username","domain","password"))
            {
                Console.WriteLine("Logged in user: " + System.Environment.UserName.ToString());
                
            }
            Console.WriteLine("Logged in User:" + Environment.UserName.ToString());
        }
    }
}
