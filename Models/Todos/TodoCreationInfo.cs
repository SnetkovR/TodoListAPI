using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Todos
{
    public class TodoCreationInfo
    {
        public TodoCreationInfo(Guid userId, string title, string text, IEnumerable<string> tags = null)
        {
            this.UserId = userId;
            this.Title = title ?? throw new ArgumentNullException(nameof(title));
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
            this.Tags = tags?.ToArray() ?? new string[] { };
        }

        /// <summary>
        /// Идентификатор пользователя, который хочет создать заметку
        /// </summary>
        public Guid UserId { get; }

        /// <summary>
        /// Заголовок задачи
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Текст задачи
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Теги задачи
        /// </summary>
        public IReadOnlyList<string> Tags { get; }
    }
}