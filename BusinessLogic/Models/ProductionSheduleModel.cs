namespace BusinessLogic.Models
{
    public class ProductionSheduleModel
    {
        public List<Event> Events { get; set; } = new();
    }

    public class Event
    {
        public string Text { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
    }
}
