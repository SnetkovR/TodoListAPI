using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Models;
using Models.Todos;
using Models.Todos.Repository;
using MongoDB.Driver;

namespace ToDoList.Services
{
    public class TodoService : ITodoRepository
    {
        private readonly IMongoCollection<Todo> _todos;

        public TodoService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("TodoDB"));
            var database = client.GetDatabase("TodoDB");
            _todos = database.GetCollection<Todo>("Todos");
        }

        public List<Todo> Get()
        {
            return _todos.Find(record => true).ToList();
        }

        public Todo Get(Guid id)
        {
            return _todos.Find<Todo>(record => record.Id == id).FirstOrDefault();
        }

        public Task<TodoInfo> CreateAsync(TodoCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var now = DateTime.UtcNow;
            var note = new Todo
            {
                Id = Guid.NewGuid(),
                UserId = creationInfo.UserId,
                CreatedAt = now,
                LastUpdatedAt = now,
                Title = creationInfo.Title,
                Text = creationInfo.Text,
                Tags = creationInfo.Tags
            };

            _todos.InsertOneAsync(note, cancellationToken: cancellationToken);

            return Task.FromResult<TodoInfo>(note);

        }

        public async Task<IReadOnlyList<TodoInfo>> SearchAsync(TodoInfoSearchQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var cursor = await _todos.FindAsync<Todo>(record => query.UserId == record.UserId, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            var search = cursor.ToEnumerable();
            if (query.CreatedFrom != null)
            {
                search = search.Where(note => note.CreatedAt >= query.CreatedFrom.Value);
            }

            if (query.CreatedTo != null)
            {
                search = search.Where(note => note.CreatedAt <= query.CreatedTo.Value);
            }

            if (query.UserId != null)
            {
                search = search.Where(note => note.UserId == query.UserId.Value);
            }


            if (query.Tags != null)
            {
                bool WhereTagsPresent(TodoInfo todoInfo)
                {
                    return query.Tags.All(tag => todoInfo.Tags.Contains(tag));
                }

                search = search.Where(WhereTagsPresent);
            }

            if (query.Offset != null)
            {
                search = search.Take(query.Offset.Value);
            }

            if (query.Limit != null)
            {
                search = search.Take(query.Limit.Value);
            }

            var sort = query.Sort ?? SortType.Ascending;
            var sortBy = query.SortBy ?? TodoSortBy.Creation;

            if (sort != SortType.Ascending || sortBy != TodoSortBy.Creation)
            {
                DateTime select(TodoInfo note)
                {
                    switch (sortBy)
                    {
                        case TodoSortBy.LastUpdate:
                            return note.LastUpdatedAt;

                        case TodoSortBy.Creation:
                            return note.CreatedAt;

                        default:
                            throw new ArgumentException($"Unknown note sort by value \"{sortBy}\".", nameof(query));
                    }
                }

                search = sort == SortType.Ascending ?
                    search.OrderBy(select) :
                    search.OrderByDescending(select);
            }

            var result = search.ToList();

            return await Task.FromResult<IReadOnlyList<TodoInfo>>(result);
        }

        public Task<Todo> GetAsync(Guid recordId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var resultNote = _todos.Find<Todo>(note => note.Id == recordId).First();

            return Task.FromResult(resultNote);
        }

        public Task<Todo> PatchAsync(TodoPatchInfo patchInfo, CancellationToken cancellationToken)
        {
            if (patchInfo == null)
            {
                throw new ArgumentNullException(nameof(patchInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var resultNote = _todos.Find<Todo>(record => record.Id == patchInfo.RecordId).First();

            if (resultNote == null)
            {
                // TODO исключение notenotfound
            }

            var updated = false;

            if (patchInfo.Title != null)
            {
                resultNote.Title = patchInfo.Title;
                updated = true;
            }

            if (patchInfo.Text != null)
            {
                resultNote.Text = patchInfo.Text;
                updated = true;
            }

            if (updated)
            {
                resultNote.LastUpdatedAt = DateTime.UtcNow;
            }

            return Task.FromResult(resultNote);
        }

        public Task RemoveAsync(Guid noteId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var resultNote = _todos.Find<Todo>(note => note.Id == noteId).First();

            _todos.DeleteOneAsync<Todo>(note => note.Id == noteId, cancellationToken: cancellationToken);

            return Task.CompletedTask;
        }
    }
}
