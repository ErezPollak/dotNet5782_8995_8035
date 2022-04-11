//course: Mini Project in Windows Systems
//lecturer: Eliezer Grintsborger
//from the students: Erez Polak 322768995


//the program contains a struct class that represents a connection between a drone and a base station.
//the program contains the properties and a toString function.


using System;

namespace DO
{
    public readonly struct DroneCharge
    {
        public int DroneId { get; init; }

        public int StationId { get; init; }

        public DateTime? EntryIntoCharge{ get; init; }
        
        /// <summary>
        /// the function returns the current place of the item's properties.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"DroneCharge: droneId: {DroneId} , stationId {StationId}\n";
        }
    }
}