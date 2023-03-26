namespace DataAcess.Entities
{
    public class StationEntity
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Instrument { get; set; } = string.Empty;
        public int SetupTimeInSecond { get; set; }
        public int OperationTimeInSecond { get; set; }
        public int TransferTimeInSecond { get; set; }
    }
}
