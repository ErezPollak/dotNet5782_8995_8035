//course: Mini Project in Windows Systems
//lecturere: Eliezer Grintsborger
//from the students: Erez Polak 322768995
//                   Mordehay Cohen 206958035


//the program contains a struct class that represents a parcel.
//the program contains the properties and a tostring function.


using System;

namespace DO
{
    public struct Parcel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public int DroneId { get; set; }
        public DateTime? DefinededTime { get; set; }
        public DateTime? AssigndedTime { get; set; }
        public DateTime? PickedUpTime { get; set; }
        public DateTime? DeliveryTime { get; set; }
        
       

        /// <summary>
        /// the function prints all the props of the struct.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //the function returns the current place of the item's properties.///

            return $"Parcel: id: {Id} , senderld: {SenderId} , targetld : {TargetId} " +
                   $", weight: {Weight}, priority: {Priority},  droneld: {DroneId}, " +
                   $"defined: {DefinededTime} , assigned: {AssigndedTime} ,  " +
                   $"pickedUp: {PickedUpTime} , deliverd: {DeliveryTime} ,  \n";
        }
    }
}