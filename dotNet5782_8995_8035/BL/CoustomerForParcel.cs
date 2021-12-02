namespace IBAL
{
    namespace BO
    {
        public class CoustomerForParcel
        {
            public int Id { get; set; }
            public string CustomerName { get; set; }

            public override string ToString()
            {
                return $"ID: {Id} " +
                    $"customer name: {CustomerName} " +
                    $"";
            }
        }
    }
}