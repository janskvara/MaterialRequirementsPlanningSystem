using DataAcess.Entities;

namespace BusinessLogic.Models
{
    public class ShiftSummaryResponseModel
    {
        public Dictionary<string, List<ShiftSummaryEntity>> ShiftsSummary { get; set; } = new();
    }
}
