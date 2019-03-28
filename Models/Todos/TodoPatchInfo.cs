using System;

namespace Models.Todos
{
    public class TodoPatchInfo
    {
        /// <summary>
        /// Создает новый экземпляр объекта, описывающего изменение заметки
        /// </summary>
        /// <param name="recordId">Идентификатор задачи, которую нужно изменить</param>
        /// <param name="title">Новый заголовок задачи</param>
        /// <param name="text">Новый текст задачи</param>
        public TodoPatchInfo(Guid recordId, string title = null, string text = null)
        {
            this.RecordId = recordId;
            this.Title = title;
            this.Text = text;
        }

        /// <summary>
        /// Идентификатор задачи, которую нужно изменить
        /// </summary>
        public Guid RecordId { get; }

        /// <summary>
        /// Новый заголовок задачи
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Новый текст задачи
        /// </summary>
        public string Text { get; set; }
    }
}