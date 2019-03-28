using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Todos.Repository
{
    public interface ITodoRepository
    {
        Task<TodoInfo>  CreateAsync(TodoCreationInfo creationInfo, CancellationToken cancellationToken);

        Task<IReadOnlyList<TodoInfo>> SearchAsync(TodoInfoSearchQuery query, CancellationToken cancellationToken);

        Task<Todo> GetAsync(Guid recordId, CancellationToken cancellationToken);

        Task<Todo> PatchAsync(TodoPatchInfo patchInfo, CancellationToken cancellationToken);

        Task RemoveAsync(Guid recordId, CancellationToken cancellationToken);
    }
}