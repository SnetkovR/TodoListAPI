using System.Runtime.Serialization;

namespace ClientModels.Notes
{
    public class NotePatchInfo
    {
        /// <summary>
        /// Флаг, указывающий, что заявка находится в избранном
        /// </summary>
        [DataMember(IsRequired = false)]
        public bool? Favorite { get; set; }

        /// <summary>
        /// Новый заголовок заметки
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Title { get; set; }

        /// <summary>
        /// Новый текст заметки
        /// </summary>
        [DataMember(IsRequired = false)]
        public string Text { get; set; }
    }
}