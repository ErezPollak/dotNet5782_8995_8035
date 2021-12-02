namespace IBAL
{
    namespace BO
    {
        public class BaseStationForList
        {
            
            public int Id { get; set; }
            public string Name { get; set; }
            public int FreeChargingSlots { get; set; }
            public int TakenCharingSlots { get; set; }

            
            public override string ToString()
            {
                return $"ID: {Id} " +
                    $"Name: {Name} " +
                    $"Free Charging Slots: {FreeChargingSlots} " +
                    $"Taken Charging Slots: {TakenCharingSlots} " +
                    $"";

            }
        }
    }
}
