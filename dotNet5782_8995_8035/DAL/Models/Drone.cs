//course: Mini Project in Windows Systems
//lecturer: Eliezer Grintsborger
//from the students: Erez Polak 322768995


//the program contains a struct class that represents a drone.
//the program contains the properties and a toString function.

    namespace DO
    {
        public struct Drone
        {
            public int Id { get; init; }
            public string Model { get; set; }
            public WeightCategories MaxWeight{ get; init; }
           
            /// <summary>
            /// the function returns the current place of the item's properties.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return $"Drone: id: {Id} , model: {Model} , MaxWeight: {MaxWeight} \n";
            }

        }
    }