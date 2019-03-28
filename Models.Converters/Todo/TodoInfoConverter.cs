using System;
using System.Linq;
using Client = global::ClientModels.Todos;
using Model = global::Models.Todos;

namespace Models.Converters.Todo
{
    public static class TodoInfoConverter
    {
        public static Client.TodoInfo Convert(Model.TodoInfo modelNoteInfo)
        {
            if (modelNoteInfo == null)
            {
                throw new ArgumentNullException(nameof(modelNoteInfo));
            }

            var clientNoteInfo = new Client.TodoInfo()
            {
                Id = modelNoteInfo.Id.ToString(),
                UserId = modelNoteInfo.UserId.ToString(),
                CreatedAt = modelNoteInfo.CreatedAt,
                LastUpdatedAt = modelNoteInfo.LastUpdatedAt,
                Title = modelNoteInfo.Title,
                Tags = modelNoteInfo.Tags.ToList()
            };

            return clientNoteInfo;
        }
    }
}