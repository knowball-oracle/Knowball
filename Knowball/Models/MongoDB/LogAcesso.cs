using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Fiap.Knowball.Models.MongoDB
{
    public class LogAcesso
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Usuario { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string Metodo { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public long TempoRespostaMs { get; set; }
        public DateTime DataHora { get; set; } = DateTime.UtcNow;
        public string? IpOrigem { get; set; }
    }
}
