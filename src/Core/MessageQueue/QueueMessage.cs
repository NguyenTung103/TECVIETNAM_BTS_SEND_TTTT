using System.Collections.Generic;
using System.Text.Json;

namespace Core.Entities
{
    public class QueueMessage
    {
        public string Id { get; set; }
        public Dictionary<string, string> Datas { get; set; } = new Dictionary<string, string>();
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
        public static QueueMessage LoadFromJson(string json)
        {
            return JsonSerializer.Deserialize<QueueMessage>(json);
        }
    }
}