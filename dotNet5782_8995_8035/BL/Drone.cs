namespace IBAL
{
    namespace BO
    {
        public class Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public Enums.WeightCategories MaxWeight { get; set; }
            public double Battery { get; set; }
            public Enums.DroneStatuses Status { get; set; }
            public ParcelInDelivery ParcelInDelivery { get; set; }
            public Location Location { get; set; }

            /// <summary>
            /// the function prints all the props of the struct.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return $"ID: {Id} \n" +
                    $"Model: {Model} \n" +
                    $"max Weight: {MaxWeight} \n" +
                    $"status: {Status}\n" +
                    $"battery: {Battery} \n" +
                    $"parcel in delivery: {ParcelInDelivery} \n" +
                    $"location: {Location} \n" +
                    $"";
            }
        }
    }
}
