namespace IBAL
{
    namespace BO
    {
        public class ParcelByCustomer
        {
            public int Id { get; set; }
            public Enums.WeightCategories Weight { get; set; }
            public Enums.Priorities Priority { get; set; }
            public Enums.ParcelStatus Status { get; set; }
            public CoustomerForParcel SenderOrReciver { get; set; }

            public override string ToString()
            {
                return $"Weight: {Weight} " +
                    $"Priority: {Priority} " +
                    $"Status: {Status} " +
                    $"SenderOrReciver: {SenderOrReciver} " +
                    $"";
            }


        }
    }
}
