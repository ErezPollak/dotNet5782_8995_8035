namespace IBAL
{
    namespace BO
    {
        public class DroneForList
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public Enums.WeightCategories Weight { get; set; }
            public double Battery { get; set; }
            public Enums.DroneStatuses Status { get; set; }
            public Location Location { get; set; }
            public int ParcelId { get; set; }

            public override string ToString()
            {
                return $"ID: {Id} \n" +
                    $"model: {Model} \n" +
                    $"weight: {Weight} \n" +
                    $"battery: {Battery} \n" +
                    $"status: {Status} \n" +
                    $"location: {Location} \n" +
                    $"parcelId: {ParcelId}\n" +
                    $"";
            }

        }
    }
}