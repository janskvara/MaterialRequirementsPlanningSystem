namespace DataAcess.Entities
{
    public class OrderPlanEntity
    {
        public int OrderId { get; set; }
        public string ModelId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime ExportDate { get; set; }
    }
}
