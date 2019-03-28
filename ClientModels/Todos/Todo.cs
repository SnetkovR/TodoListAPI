using System;

namespace ClientModels.Todos
{
    /// <summary>
    /// Задача
    /// </summary>
    public class Todo : TodoInfo
    {
        /// <summary>
        /// Описание задачи
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата, к которой задача должна быть выполнена
        /// </summary>
        public DateTime? EndAt { get; set; }
    }
}