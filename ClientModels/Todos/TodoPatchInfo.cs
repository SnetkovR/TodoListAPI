using System;
using System.Runtime.Serialization;

namespace ClientModels.Todos
{
    [DataContract]
    public class TodoPatchInfo
    {
        /// <summary>
        /// Новый заголовок задачи
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Title { get; set; }

        /// <summary>
        /// Новый текст задачи
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Text { get; set; }

        /// <summary>
        /// Новая дата закрытия
        /// </summary>
        [DataMember(IsRequired = false)]
        public DateTime EndAt { get; set; }
    }
}