using MongoDB.Bson.Serialization.Attributes;

namespace DataAcess.Entities
{
    public class RouteSheetEntity
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Department { get; set; }
        public List<int> StationList { get; set; } = new List<int>();
    }
}
