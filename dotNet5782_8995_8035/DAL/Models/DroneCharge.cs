//course: Mini Project in Windows Systems
//lecturere: Eliezer Grintsborger
//from the students: Erez Polak 322768995
//                   Mordehay Cohen 206958035


//the program contains a struct class that represents a connection between a drone and a base station.
//the program contains the properties and a tostring function.


namespace DO
{
    public struct DroneCharge
    {
        public int DroneId { get; set; }

        public int StationId { get; set; }

        /// <summary>
        /// the function prints all the props of the struct.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //the function returns the current place of the item's properties.///

            return $"DroneCharge: droneld: {DroneId} , stationled {StationId}\n";
        }
    }
}