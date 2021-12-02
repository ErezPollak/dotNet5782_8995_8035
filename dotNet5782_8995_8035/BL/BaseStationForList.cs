namespace IBAL
{
    namespace BO
    {
        public record BaseStationForList
        (
            int Id = default,
            string Name = default,
            int FreeChargingSlots = default,
            int TakenCharingSlots = default
        )
        {
            public override string ToString() =>
                $"ID: {Id} " +
                $"Name: {Name} " +
                $"Free Charging Slots: {FreeChargingSlots} " +
                $"Taken Charging Slots: {TakenCharingSlots} " +
                $"";
        }
    }
}