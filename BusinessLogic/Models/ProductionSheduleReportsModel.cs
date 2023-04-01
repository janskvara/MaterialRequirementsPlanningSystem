using DataAcess.Entities;

namespace BusinessLogic.Models
{
    public class ProductionSheduleReportsModel
    {
        public Dictionary<string, ProductionSheduleReportEntity> Reports { get; set; } = new();
    }
}
