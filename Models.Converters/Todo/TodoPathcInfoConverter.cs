using System;
using Client = global::ClientModels.Todos;
using Model = global::Models.Todos;

namespace Models.Converters.Todo
{
    public static class TodoPathcInfoConverter
    {
        public static Model.TodoPatchInfo Convert(Guid recordId, Client.TodoPatchInfo clientPatchInfo)
        {
            if (clientPatchInfo == null)
            {
                throw new ArgumentNullException(nameof(clientPatchInfo));
            }

            var modelPatchInfo = new Model.TodoPatchInfo(recordId)
            {
                Text = clientPatchInfo.Text,
                Title = clientPatchInfo.Title
            };

            return modelPatchInfo;
        }
    }
}