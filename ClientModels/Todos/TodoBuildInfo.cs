using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ClientModels.Todos
{
    /// <summary>
    /// Информация для создания задачи
    /// </summary>
    [DataContract]
    public class TodoBuildInfo
    {
        /// <summary>
        /// Заголовок задачи
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        /// <summary>
        /// Текст задачи
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Text { get; set; }

        /// <summary>
        /// Теги задачи
        /// </summary>
        [DataMember(IsRequired = false)]
        public IReadOnlyList<string> Tags { get; set; }
    }
}