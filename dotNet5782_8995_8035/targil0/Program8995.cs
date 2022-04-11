using System;

namespace ConsoleApp1
{
    static partial class Program
    {
        private static void Main(string[] args)
        {

            for (var i = 1; i < 10; i++)
            {
                for (var j = 1; j < 10; j++)
                {
                    Console.Write("{0 , -3}", i * j);
                }
                Console.WriteLine("");
            }

            Console.WriteLine();

            var r = new Random();
            for (var i = 0; i < 100; i++)
            {
                Console.Write($"{(int)(r.Next() % 8) + 8,-10} ");
            }

        }

        static partial void Welcome8035();

        private static string Welcome8995()
        {
            Console.WriteLine("Enter your name: ");
            var s = Console.ReadLine();
            return s;
        }
    }
}
