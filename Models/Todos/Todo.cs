using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Todos
{
    public class Todo : TodoInfo
    {
        [BsonElement("Text")]
        public string Text { get; set; }

        /// <summary>
        /// Дата, к которой задача должна быть выполнена
        /// </summary>
        [BsonElement("EndAt")]
        public DateTime? EndAt { get; set; }
    }
}