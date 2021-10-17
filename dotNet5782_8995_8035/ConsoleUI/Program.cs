using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
           
            IDAL.DO.BaseStation b = new IDAL.DO.BaseStation(1, 1, r.Next() % 1000 - 500, r.Next() % 1000 - 500, r.Next() % 5);

            Console.WriteLine(r.Next() % 1000 - 500);

            Console.WriteLine(b.toString());


        }
    }
}
