namespace DataAcess.Entities
{
    public class EventEntity
    {
        public string Text { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public int Color { get; set; }

        public EventEntity(string text, string description, DateTimeOffset startTime, DateTimeOffset endTime, int color) 
        {
            Text= text;
            Description= description;
            StartTime= startTime;
            EndTime= endTime;
            Color = color;
        }
    }
}
