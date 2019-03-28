using System.Collections.Generic;

namespace ClientModels.Todos
{
    public class TodoList
    {
        /// <summary>
        /// Краткая информация о задачах
        /// </summary>
        public IReadOnlyCollection<TodoInfo> Todos { get; set; }
    }
}