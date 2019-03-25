using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Models.Notes;
using Models.Notes.Repository;
using MongoDB.Driver;

namespace ToDoList.Services
{
    public class NoteService : INoteRepository
    {
        private readonly IMongoCollection<Note> _notes;

        public NoteService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("TodoDB"));
            var database = client.GetDatabase("TodoDB");
            _notes = database.GetCollection<Note>("Notes");
        }

        public List<Note> Get()
        {
            return _notes.Find(note => true).ToList();
        }

        public Note Get(string id)
        {
            return _notes.Find<Note>(note => note.Id == id).FirstOrDefault();
        }

        public Note Create(Note note)
        {
            _notes.InsertOne(note);
            return note;
        }

        public void Update(string id, Note noteIn)
        {
            _notes.ReplaceOne(note => note.Id == id, noteIn);
        }

        public void Remove(Note noteIn)
        {
            _notes.DeleteOne(note => note.Id == noteIn.Id);
        }

        public void Remove(string id)
        {
            _notes.DeleteOne(note => note.Id == id);
        }

        public Task<NoteInfo> CreateAsync(NoteCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            var now = DateTime.UtcNow;
            var note = new Note
            {
                Id = id,
                UserId = creationInfo.UserId,
                CreatedAt = now,
                LastUpdatedAt = now,
                Favorite = false,

                Text = creationInfo.Text,
                Tags = creationInfo.Tags
            };

            _notes.InsertOneAsync(note, cancellationToken: cancellationToken);

            return Task.FromResult<NoteInfo>(note);

        }

        public Task<IReadOnlyList<NoteInfo>> SearchAsync(NoteInfoSearchQuery query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Note> GetAsync(string noteId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var resultNote = _notes.Find<Note>(note => note.Id == noteId).First();
            if (resultNote == null)
            {
                // TODO исключение notenotfound
            }

            return Task.FromResult(resultNote);
        }

        public Task<Note> PatchAsync(NotePatchInfo patchInfo, CancellationToken cancellationToken)
        {
            if (patchInfo == null)
            {
                throw new ArgumentNullException(nameof(patchInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var resultNote = _notes.Find<Note>(note => note.Id == patchInfo.NoteId).First();

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

            if (patchInfo.Favorite != null)
            {
                resultNote.Favorite = patchInfo.Favorite.Value;
                updated = true;
            }

            if (updated)
            {
                resultNote.LastUpdatedAt = DateTime.UtcNow;
            }

            return Task.FromResult(resultNote);
        }

        public Task RemoveAsync(string noteId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var resultNote = _notes.Find<Note>(note => note.Id == noteId).First();

            _notes.DeleteOneAsync<Note>(note => note.Id == noteId, cancellationToken: cancellationToken);

            return Task.CompletedTask;
        }
    }
}
