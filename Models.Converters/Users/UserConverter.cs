using System;
using System.Collections.Generic;
using System.Text;
using Client = global::ClientModels.Users;
using Model = global::Models.Users;

namespace Models.Converters.Users
{
    class UserConverter
    {
        public static Client.User Convert(Model.User modelUser)
        {
            if (modelUser == null)
            {
                throw new ArgumentNullException(nameof(modelUser));
            }

            var clientUser = new Client.User
            {
                Id = modelUser.Id.ToString(),
                Login = modelUser.Login,
                RegisteredAt = modelUser.RegisteredAt
            };

            return clientUser;
        }
    }
}
