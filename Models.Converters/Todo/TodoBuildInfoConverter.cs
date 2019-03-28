using System;
using Client = global::ClientModels.Todos;
using Model = global::Models.Todos;

namespace Models.Converters.Todo
{
    public static class TodoBuildInfoConverter
    {
        public static Model.TodoCreationInfo Convert(string clientUserId, Client.TodoBuildInfo clientBuildInfo)
        {
            if (clientUserId == null)
            {
                throw new ArgumentNullException(nameof(clientUserId));
            }

            if (clientBuildInfo == null)
            {
                throw new ArgumentNullException(nameof(clientBuildInfo));
            }

            if (!Guid.TryParse(clientUserId, out var modelUserId))
            {
                throw new ArgumentException($"The client user id \"{clientUserId}\" is invalid.", nameof(clientUserId));
            }

            var modelCreationInfo = new Model.TodoCreationInfo(
                modelUserId,
                clientBuildInfo.Title,
                clientBuildInfo.Text,
                clientBuildInfo.Tags);

            return modelCreationInfo;
        }
    }
}