//course: Mini Project in Windows Systems
//lecturer: Eliezer Grintsborger
//from the students: Erez Polak 322768995

namespace DO
{
    public class Location
    {
        public double Longitude { get; init; }
        public double Latitude { get; init; }

        public override string ToString()
        {
            return
               $"longitude: {(int)Longitude}° {(int)((Longitude - (int)Longitude) * 60)}' {((Longitude - (int)Longitude) * 60 - (int)((Longitude - (int)Longitude) * 60)) * 60}'' S ," +
               $" latitude: {(int)Latitude}° {(int)((Latitude - (int)Latitude) * 60)}' {((Latitude - (int)Latitude) * 60 - (int)((Latitude - (int)Latitude) * 60)) * 60}'' H ,";
        }
    }
}


