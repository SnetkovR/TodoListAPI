using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Notes
{
    public class NoteInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserId")]
        public Guid UserId { get; set; }

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("LastUpdatedAt")]
        public DateTime LastUpdatedAt { get; set; }

        [BsonElement("Favorite")]
        public bool Favorite { get; set; }

        [BsonElement("Title")]
        public string Title { get; set; }

        [BsonElement("Tags")]
        public IReadOnlyList<string> Tags { get; set; }
    }
}