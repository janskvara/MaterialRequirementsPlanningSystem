namespace DataAcess.Entities
{
    public class EventEntity
    {
        public string Text { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Color { get; set; }

        public EventEntity(string text, string description, DateTime startTime, DateTime endTime, int color) 
        {
            Text= text;
            Description= description;
            StartTime= startTime;
            EndTime= endTime;
            Color = color;
        }
    }
}
