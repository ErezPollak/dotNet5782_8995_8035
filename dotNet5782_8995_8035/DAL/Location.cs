namespace IDAL
    {
        namespace DO
        {
            public class Location
            {
                public double Longitude { get; set; }
                public double Lattitude { get; set; }

                public override string ToString()
                {
                    return
                       $"longitude: {(int)(Longitude)}° {(int)((Longitude - (int)(Longitude)) * 60)}' {((Longitude - (int)(Longitude)) * 60 - (int)((Longitude - (int)(Longitude)) * 60)) * 60}'' S ," +
                       $" lattitude: {(int)(Lattitude)}° {(int)((Lattitude - (int)(Lattitude)) * 60)}' {((Lattitude - (int)(Lattitude)) * 60 - (int)((Lattitude - (int)(Lattitude)) * 60)) * 60}'' H ,";
                }
            }
        }
    }


