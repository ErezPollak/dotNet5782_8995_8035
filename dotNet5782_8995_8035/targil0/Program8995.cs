using System;

namespace ConsoleApp1
{
    partial class Program
    {
        static void Main(string[] args)
        {

            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    Console.Write("{0 , -3}", i * j);
                }
                Console.WriteLine("");
            }

            Console.WriteLine();

            Random r = new Random();
            for (int i = 0; i < 100; i++)
            {
                Console.Write($"{(int)(r.Next() % 8) + 8,-10} ");
            }

        }

        static partial void Welcome8035();

        private static string Welcome8995()
        {
            string s;

            Console.WriteLine("Enter your name: ");
            s = Console.ReadLine();
            //Console.WriteLine($"{} welcome to my first console application" , s);
            return s;
        }
    }
}
