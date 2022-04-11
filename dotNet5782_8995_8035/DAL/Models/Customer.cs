//course: Mini Project in Windows Systems
//lecturer: Eliezer Grintsborger
//from the students: Erez Polak 322768995



//the program contains a struct that represents a customer.
//the program contains the properties and a toString function.


namespace DO
    {
        public struct Customer
        {
            public int Id { get; init; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public Location Location{ get; init; }

            /// <summary>
            /// the function prints all the props of the struct.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {

                return $"Customer: " +
                    $"id: {Id} , " +
                    $"name: {Name} , " +
                    $"phone : {Phone } , " +
                    $"location: {Location}" +
                    $"";
            }

        }
    }
