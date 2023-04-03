namespace DataAcess.Entities
{
    public class OrderGoodsEntities
    {
        public DateTimeOffset ImportDate { get; set; }
        public List<Order> Orders { get; set; } = new();

        public OrderGoodsEntities(DateTimeOffset importDate)
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
