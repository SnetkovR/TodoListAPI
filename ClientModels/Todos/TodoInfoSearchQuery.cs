using System;
using System.Collections.Generic;

namespace ClientModels.Todos
{
    /// <summary>
    /// Параметры поиска задач
    /// </summary>
    public class TodoInfoSearchQuery
    {
        /// <summary>
        /// Позиция, начиная с которой нужно производить поиск
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Максимальное количество задач, которое нужно вернуть
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Пользователь, которому принадлежит задача
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Минимальная дата создания задачи
        /// </summary>
        public DateTime? CreatedFrom { get; set; }

        /// <summary>
        /// Максимальная дата создания задач
        /// </summary>
        public DateTime? CreatedTo { get; set; }

        /// <summary>
        /// Тип сортировки
        /// </summary>
        public SortType? Sort { get; set; }

        /// <summary>
        /// Аспект задач, по которому нужно искать
        /// </summary>
        public TodoSortBy? SortBy { get; set; }

        /// <summary>
        /// Теги задач
        /// </summary>
        public IReadOnlyList<string> Tags { get; set; }
    }
}