namespace IBAL
{
    namespace BO
    {
        public record Location
        (
            double Longitude = default,
            double Latitude = default
        )
        {
            public override string ToString() =>
                $"longitude: {(int) (Longitude)}° {(int) ((Longitude - (int) (Longitude)) * 60)}' {((Longitude - (int) (Longitude)) * 60 - (int) ((Longitude - (int) (Longitude)) * 60)) * 60}'' S ," +
                $" lattitude: {(int) (Latitude)}° {(int) ((Latitude - (int) (Latitude)) * 60)}' {((Latitude - (int) (Latitude)) * 60 - (int) ((Latitude - (int) (Latitude)) * 60)) * 60}'' H ,";
        }
    }
}