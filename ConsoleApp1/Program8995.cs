using System;

namespace ConsoleApp1
{
    partial class Program
    {
        static void Main(string[] args)
        {
            string s = "";
            s = Welcome8995();
            Welcome8035();
        }

        static partial void Welcome8035();

        private static string Welcome8995()
        {
            string s;

            //my sencear apolagies

            Console.WriteLine("Enter your name: ");
            s = Console.ReadLine();
            Console.WriteLine(s + " welcome to my first console application");
            return s;
        }
    }
}
