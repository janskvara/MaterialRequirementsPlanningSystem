using DataAcess.Entities;

namespace BusinessLogic.Models
{
    public class ManufactorDay
    {
        public DateTime Date { get; set; }
        public List<OccupiedShift> ShiftsOfDay { get; set; } = new ();
        public bool IsFullyOccupied { get; set; }
    }

    public class OccupiedShift
    {
        public bool IsFullyOccupied { get; set; }
        public Shift? Shift { get; set; }
    }
}
