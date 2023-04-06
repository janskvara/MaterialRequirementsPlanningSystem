namespace DataAcess.Entities
{
    public class OrderGoodsEntities
    {
        public DateTime ImportDate { get; set; }
        public List<Order> Orders { get; set; } = new();

        public OrderGoodsEntities(DateTime importDate)
        {
            ImportDate= importDate;
        }
    }

    public class Order
    {
        public string PartNumber { get; set; }
        public double Count { get; set; }

        public Order(string partNumber, double count)
        {
            PartNumber= partNumber;
            Count= count;
        }
    }
}
