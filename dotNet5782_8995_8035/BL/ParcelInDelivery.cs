namespace IBAL
{
    namespace BO
    {
        public class ParcelInDelivery
        {
            public int Id { get; set; }
            public Enums.ParcelStatus Status { get; set; }
            public Enums.Priorities Priority { get; set; }
            public Enums.WeightCategories Weight { get; set; }
            public CoustomerForParcel Sender { get; set; }
            public CoustomerForParcel Reciver { get; set; }
            public Location PickupLockation { get; set; }
            public Location DeliveringLockation { get; set; }
            public double Distance { get; set; }

            public override string ToString()
            {
                return $"Id: {Id} " +
                   $"Status: {Status} " +
                   $"Priority: {Priority} " +
                   $"Weight: {Weight} " +
                   $"Sender: {Sender} " +
                   $"reciver: {Reciver} " +
                   $"PickupLockation: {PickupLockation} " +
                   $"Distance: {Distance} " +
                   $"";
            }


        }
    }
}
