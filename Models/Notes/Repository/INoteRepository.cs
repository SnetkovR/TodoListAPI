using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Notes.Repository
{
    public interface INoteRepository
    {
        Task<NoteInfo>  CreateAsync(NoteCreationInfo creationInfo, CancellationToken cancellationToken);

        Task<IReadOnlyList<NoteInfo>> SearchAsync(NoteInfoSearchQuery query, CancellationToken cancellationToken);

        Task<Note> GetAsync(string noteId, CancellationToken cancellationToken);

        Task<Note> PatchAsync(NotePatchInfo patchInfo, CancellationToken cancellationToken);

        Task RemoveAsync(string noteId, CancellationToken cancellationToken);
    }
}