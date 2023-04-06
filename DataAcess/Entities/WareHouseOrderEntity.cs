namespace DataAcess.Entities
{
    public class WareHouseOrderEntity
    {   
        public WareHouseOrderEntity(string planReportId, int orderId, OrderGoodsEntities orders) 
        {
            PlanReportId = planReportId;
            OrderId = orderId;
            ImportDate= orders.ImportDate;
            ImportOrders = orders.Orders;
        }

        public string PlanReportId { get; set; }
        public int OrderId { get; set; }
        public DateTime ImportDate { get; set; }
        public List<Order> ImportOrders { get; set; } = new(); 
    }
}
