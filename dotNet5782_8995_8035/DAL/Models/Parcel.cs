//course: Mini Project in Windows Systems
//lecturer: Eliezer Grintsborger
//from the students: Erez Polak 322768995


//the program contains a struct class that represents a parcel.
//the program contains the properties and a toString function.


using System;

namespace DO
{
    public struct Parcel
    {
        public int Id { get; init; }
        public int SenderId { get; init; }
        public int TargetId { get; init; }
        public WeightCategories Weight { get; init; }
        public Priorities Priority { get; init; }
        public int DroneId { get; set; }
        public DateTime? DefinedTime { get; init; }
        public DateTime? AssignedTime { get; set; }
        public DateTime? PickedUpTime { get; set; }
        public DateTime? DeliveryTime { get; set; }
        
       

        /// <summary>
        /// the function prints all the props of the struct.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //the function returns the current place of the item's properties.///

            return $"Parcel: id: {Id} , senderId: {SenderId} , targetId : {TargetId} " +
                   $", weight: {Weight}, priority: {Priority},  droneId: {DroneId}, " +
                   $"defined: {DefinedTime} , assigned: {AssignedTime} ,  " +
                   $"pickedUp: {PickedUpTime} , delivered: {DeliveryTime} ,  \n";
        }
    }
}