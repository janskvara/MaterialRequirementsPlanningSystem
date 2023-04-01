using DataAcess.Entities;

namespace BusinessLogic.Models
{
    public class OrderPlanRequest
    {
        public string ReportId { get; set; } = string.Empty;
        public List<OrderPlanEntity> Orders { get; set; } = new();
        internal DateTimeOffset FromDate { get; set; } = new DateTimeOffset(new DateTime(2023, 03, 1, 0, 0, 0, DateTimeKind.Utc));
    }
}
