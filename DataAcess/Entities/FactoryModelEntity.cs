using DataAcess.Entities.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAcess.Entities
{
    public class FactoryModelEntity
    {
        [BsonId]
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public ModelType ModelType { get; set; }
        public int RouteSheetId { get; set; }
        public int BillOfMaterialId { get; set; }
    }
}
