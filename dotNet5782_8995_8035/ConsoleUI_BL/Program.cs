using IBL;
using System;

namespace ConsoleUI_BL
{
    class Program
    {
        static void Main(string[] args)
        {
            IBL.BL bl = new BL();

            IBAL.BO.DroneForList drone = new IBAL.BO.DroneForList()
            {
                Id = 200,
                Battary = 100,
                Location = new IBAL.BO.Location()
                {
                    Lattitude = 10,
                    Longitude = 20
                },
                Model = "AA11111",
                ParcelId = 3,
                Weight = 0,
                Status = (IBAL.BO.Enums.DroneStatuses)1
            };

            bl.AddDrone(drone);

            foreach(IBAL.BO.DroneForList d in bl.GetDrones())
            {
                Console.WriteLine(d);
            }

        }
    }
}
