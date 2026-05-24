using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Fiap.Knowball.Models;

public class DenunciaLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public int DenunciaId { get; set; }
    public string Acao { get; set; } = string.Empty;
    public string? Detalhes { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Usuario { get; set; }
}