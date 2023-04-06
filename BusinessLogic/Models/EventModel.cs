using DataAcess.Entities;
using System.Text;

namespace BusinessLogic.Models
{
    public abstract class EventModel
    {
        public abstract EventEntity Event { get; }

        public abstract int OrderId { get; }

        protected string GetDescription(params string[] descriptions)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string description in descriptions)
            {
                sb.Append(description);
                sb.Append("-----");
            }
            return sb.ToString();
        }
    }

    public class ManufacturesEventModel: EventModel
    {
        private readonly EventEntity _event; 
        private readonly int _orderId;

        public override EventEntity Event => _event;
        public override int OrderId => _orderId;

        public ManufacturesEventModel(int orderId, string model, double quantity, DateTime startTime, DateTime endTime)
        {
            var color = 1;
            _orderId = orderId;
            var text = $"Vyroba objednavky:{orderId}";
            var description = GetDescription($"Vyroba objednavky:{orderId}", $"Model: {model}",
                $"Zaciatok vyroby: {startTime}", $"Predpokladane ukoncenie vyroby: {endTime}",
                $"Celkovy pocet vyrobenych kusov: {quantity}");
            _event = new EventEntity(text, description, startTime, endTime, color);
        }
    }

    public class ExportEventModel: EventModel
    {
        private readonly EventEntity _event;
        private readonly int _orderId;

        public override EventEntity Event => _event;
        public override int OrderId => _orderId;

        public ExportEventModel(int orderId, string model, DateTime startTime)
        {
            var color = 2;
            _orderId = orderId;
            var text = $"Export objednavky:{orderId}";
            var description = GetDescription($"Export objednavky:{orderId}", $"Model: {model}",
                 $"Export objednavky: {startTime}");
            _event = new EventEntity(text, description, startTime, startTime, color);
        }
    }

    public class ImportGoodsEventModel : EventModel
    {
        private readonly EventEntity _event;
        private readonly int _orderId;

        public override EventEntity Event => _event;
        public override int OrderId => _orderId;

        public ImportGoodsEventModel(int orderId, string model, DateTime deadline, List<Order> orders)
        {
            var color = 3;
            _orderId = orderId;
            var text = $"Import tovaru:{orderId}";
            var ordersText = orders.Select(x => $" Id:{x.PartNumber} Pocet: {x.Count}, " ).ToList();

            var description = GetDescription($"Import tovaru pre objednavku:{orderId}", $"Model: {model}",
                $"Deadline importu vyrobkov na zozname: {deadline}", $"Objednavkovy list: {String.Join(" ", ordersText)}");
            _event = new EventEntity(text, description, deadline, deadline, color);
        }
    }

    public class ShiftEvent : EventModel
    {
        private readonly EventEntity _event;
        private readonly int _orderId;

        public override EventEntity Event => _event;
        public override int OrderId => _orderId;

        public ShiftEvent(int orderId, string model, DateTime startTime, DateTime endTime)
        {
            var color = 4;
            _orderId = orderId;
            var text = $"Zmena:{startTime} - {endTime}";
            var description = GetDescription($"Zmena:{startTime} - {endTime}", $"Praca na objednavke: {orderId}",
                 $"Model: {model}");
            _event = new EventEntity(text, description, startTime, endTime, color);
        }
    }
}
