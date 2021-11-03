//course: Mini Project in Windows Systems
//lecturere: Eliezer Grintsborger
//from the students: Erez Polak 322768995
//                   Mordehay Cohen 206958035



//the program contains a struct that represents a customer.
//the program contains the properties and a tostring function.

namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Llattitude { get; set; }

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
                    $"longitude: {(int)(this.Longitude)}° {(int)((this.Longitude - (int)(this.Longitude)) * 60)}' {((this.Longitude - (int)(this.Longitude)) * 60 - (int)((this.Longitude - (int)(this.Longitude)) * 60)) * 60}'' ," +
                    $" lattitude: {(int)(this.Llattitude)}° {(int)((this.Llattitude - (int)(this.Llattitude)) * 60)}' {((this.Llattitude - (int)(this.Llattitude)) * 60 - (int)((this.Llattitude - (int)(this.Llattitude)) * 60)) * 60}'' \n";
          
            }

        }
    }
}
