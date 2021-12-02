namespace IBAL
{
    namespace BO
    {
        public class ParcelForList
        {
            public int Id { get; set; }
            public string SenderName { get; set; }
            public string ReciverName { get; set; }
            public Enums.WeightCategories Weight { get; set; }
            public Enums.Priorities Priority { get; set; }
            public Enums.ParcelStatus Status { get; set; }

            public override string ToString()
            {
                return $"Id: {Id} " +
                    $"SenderName: {SenderName} " +
                    $"ReciverName: {ReciverName} " +
                    $"Weight: {Weight} " +
                    $"Priority: {Priority} " +
                    $"Status: {Status} " +
                    $"";
            }

        }
    }
}
