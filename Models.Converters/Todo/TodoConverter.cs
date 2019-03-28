using System;
using System.Linq;
using Client = global::ClientModels.Todos;
using Model = global::Models.Todos;

namespace Models.Converters.Todo
{
    public static class TodoConverter
    {
        public static Client.Todo Convert(Model.Todo modelRecord)
        {
            if (modelRecord == null)
            {
                throw new ArgumentNullException(nameof(modelRecord));
            }

            var clientNote = new Client.Todo()
            {
                Id = modelRecord.Id.ToString(),
                UserId = modelRecord.UserId.ToString(),
                CreatedAt = modelRecord.CreatedAt,
                LastUpdatedAt = modelRecord.LastUpdatedAt,
                Title = modelRecord.Title,
                Text = modelRecord.Text,
                Tags = modelRecord.Tags.ToList(),
                EndAt = modelRecord.EndAt
            };

            return clientNote;
        }
    }
}