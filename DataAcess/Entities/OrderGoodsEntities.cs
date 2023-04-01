namespace DataAcess.Entities
{
    public class OrderGoodsEntities
    {
        public DateTimeOffset ImportDate { get; }
        public List<Order> Orders { get; set; } = new();

        public OrderGoodsEntities(DateTimeOffset importDate)
        {
            ImportDate= importDate;
        }
    }

    public class Order
    {
        public string PartNumber { get; }
        public double Count { get; }

        public Order(string partNumber, double count)
        {
            PartNumber= partNumber;
            Count= count;
        }
    }
}
