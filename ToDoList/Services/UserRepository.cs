using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Models.Users;
using Models.Users.Repository;
using MongoDB.Driver;

namespace ToDoList.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> users;

        public UserRepository(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("TodoDB"));
            var database = client.GetDatabase("TodoDB");
            users = database.GetCollection<User>("Users");
        }

        public async Task<User> CreateAsync(UserCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            var userDuplicate = await this.GetAsync(creationInfo.Login, cancellationToken);

            if (userDuplicate != null)
            {
                throw new UserDuplicationException(creationInfo.Login);
            }

            User user = new User()
            {
                Id = Guid.NewGuid(),
                Login = creationInfo.Login,
                PasswordHash = creationInfo.PasswodHash,
                RegisteredAt = DateTime.UtcNow

            };

            await users.InsertOneAsync(user, cancellationToken);
            return await Task.FromResult(user);
        }

        public async Task<User> GetAsync(Guid userId, CancellationToken cancellationToken)
        {
            var res = await users.FindAsync(user => userId == user.Id, cancellationToken: cancellationToken);
            return res.FirstOrDefault();
        }

        public async Task<User> GetAsync(string login, CancellationToken cancellationToken)
        {
            var res = await users.FindAsync(user => login == user.Login, cancellationToken: cancellationToken);
            return res.FirstOrDefault();
        }
    }
}