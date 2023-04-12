using BusinessLogic.Models;
using DataAcess.Entities;
using DataAcess.Models;

namespace BusinessLogic.MRPService
{
    internal class ShiftSummaryManager
    {
        private readonly string _reportId;

        internal List<ShiftSummaryEntity> ShiftSummaries { get; private set; } = new List<ShiftSummaryEntity>();

        internal ShiftSummaryManager(string reportId)
        {
            _reportId = reportId;
        }

        internal void AddShift(ProductInformationModel model, OrderPlanEntity order, double quantity, 
            DateTime shiftStart, DateTime shiftEnd)
        {
            var shiftBillOfMaterials = model.SummaryBillOfMaterials.Select(x => new ProductRequirement
            {
                PartNumber = x.PartNumber,
                Quantity = x.Quantity * quantity
            }).ToList();

            var summaryShift = new ShiftSummaryEntity {
                Id = $"{_reportId}-{shiftStart}-{shiftEnd}",
                ReportId = _reportId,
                OrderId = order.OrderId,
                Quantity = quantity,
                ShiftStart = shiftStart,
                ShiftEnd = shiftEnd,
                NameOfManufacturedModel = model.PartNumber,
                BillOfMaterials= shiftBillOfMaterials
            };
            ShiftSummaries.Add(summaryShift);
        }

        internal List<ShiftSummaryEntity> GetShifts(int orderId)
        {
            return ShiftSummaries.FindAll(x => x.OrderId == orderId);
        }

        internal ShiftSummaryEntity GetLastShift()
        {
            return ShiftSummaries.Last();
        }

        internal void RemoveShifts(int orderId)
        {
            ShiftSummaries.RemoveAll(x => x.OrderId == orderId);
        }
    }
}
