
namespace DO
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public override string ToString()
        {
            return
               $"longitude: {(int)Longitude}° {(int)((Longitude - (int)Longitude) * 60)}' {((Longitude - (int)Longitude) * 60 - (int)((Longitude - (int)Longitude) * 60)) * 60}'' S ," +
               $" lattitude: {(int)Latitude}° {(int)((Latitude - (int)Latitude) * 60)}' {((Latitude - (int)Latitude) * 60 - (int)((Latitude - (int)Latitude) * 60)) * 60}'' H ,";
        }
    }
}


