using System;
using System.Linq;
using Client = global::ClientModels.Todos;
using Model = global::Models.Todos;
namespace Models.Converters.Todo
{
    public static class TodoInfoSearchQueryConverter
    {
        /// <summary>
        /// Переводит запрос за заметками из клиентсокой модели в серверную
        /// </summary>
        /// <param name="clientQuery">Запрос за заметками в клиентской модели</param>
        /// <returns>Запрос за заметками в серверной модели</returns>
        public static Model.TodoInfoSearchQuery Convert(Client.TodoInfoSearchQuery clientQuery)
        {
            if (clientQuery == null)
            {
                throw new ArgumentNullException(nameof(clientQuery));
            }

            var modelUserId = (Guid?)null;

            if (clientQuery.UserId != null)
            {
                if (!Guid.TryParse(clientQuery.UserId, out var userId))
                {
                    throw new ArgumentException($"The user id \"{clientQuery.UserId}\" is invalid.", nameof(clientQuery));
                }

                modelUserId = userId;
            }


            var modelSort = clientQuery.Sort.HasValue ?
                SortTypeConverter.Convert(clientQuery.Sort.Value) :
                (Models.SortType?)null;

            var modelSortBy = clientQuery.SortBy.HasValue ?
                TodoSortByConverter.Convert(clientQuery.SortBy.Value) :
                (Model.TodoSortBy?)null;

            var modelQuery = new Model.TodoInfoSearchQuery()
            {
                CreatedFrom = clientQuery.CreatedFrom,
                CreatedTo = clientQuery.CreatedTo,
                UserId = modelUserId,
                Limit = clientQuery.Limit,
                Offset = clientQuery.Offset,
                Sort = modelSort,
                SortBy = modelSortBy,
                Tags = clientQuery.Tags?.ToList()
            };

            return modelQuery;
        }
    }
}