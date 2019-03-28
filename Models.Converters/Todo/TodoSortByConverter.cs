using System;
using Client = global::ClientModels.Todos;
using Model = global::Models.Todos;

namespace Models.Converters.Todo
{
    public class TodoSortByConverter
    {
        /// <summary>
        /// Переводит аспект сортировки заметок из клиентской модели в серверную
        /// </summary>
        /// <param name="clientSortBy">Аспект сортировки заметок в клиентской модели</param>
        /// <returns>Аспект сортировки заметок в серверной модели</returns>
        public static Model.TodoSortBy Convert(Client.TodoSortBy clientSortBy)
        {
            switch (clientSortBy)
            {
                case Client.TodoSortBy.Creation:
                    return Model.TodoSortBy.Creation;

                case Client.TodoSortBy.LastUpdate:
                    return Model.TodoSortBy.LastUpdate;

                default:
                    throw new ArgumentException($"Unknown sort by value \"{clientSortBy}\".", nameof(clientSortBy));
            }
        }
    }
}