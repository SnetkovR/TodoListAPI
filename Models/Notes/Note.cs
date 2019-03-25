using MongoDB.Bson.Serialization.Attributes;

namespace Models.Notes
{
    public class Note : NoteInfo
    {
        [BsonElement("Text")]
        public string Text { get; set; }
    }
}