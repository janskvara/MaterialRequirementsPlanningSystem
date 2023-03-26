using MongoDB.Bson.Serialization.Attributes;

namespace DataAcess.Entities
{
    public class BillOfMaterialsEntity
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<MaterialOfBill> MaterialList { get; set; } = new List<MaterialOfBill>();
    }
}
