using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Notes
{
    public class NoteCreationInfo
    {
        public NoteCreationInfo(Guid userId, string title, string text, IEnumerable<string> tags = null)
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
        /// Заголовок заметки
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Текст заметки
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Теги заметки
        /// </summary>
        public IReadOnlyList<string> Tags { get; }
    }
}