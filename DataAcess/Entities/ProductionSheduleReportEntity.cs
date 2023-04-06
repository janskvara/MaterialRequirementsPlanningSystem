using MongoDB.Bson.Serialization.Attributes;

namespace DataAcess.Entities
{
    public class ProductionSheduleReportEntity
    {
        [BsonId]
        public string ReportId { get; set; } = string.Empty;

        public DateTimeOffset FromDate { get; set; }

        public List<OrderPlanEntity> Orders { get; set; } = new(); 

        public List<EventEntity> Events { get; set; } = new();

        public List<ShiftSummaryEntity> ShiftSummaries { get; set; } = new();

        public List<string> Description { get; set; } = new();
    }
}
