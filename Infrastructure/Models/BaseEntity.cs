using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DateJournal.Infrastructure.Models
{
	public abstract class BaseEntity
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public virtual string Id { get; set; } = null!;
	}
}
