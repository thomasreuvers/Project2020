using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Project.Entities;

namespace Project.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public Task<List<User>> Get() => _users.Find(user => true).ToListAsync();

        public Task<User> GetUserByEmailTask(string emailAddress) =>
            _users.Find(user => user.EmailAddress.Equals(emailAddress)).FirstOrDefaultAsync();

        public Task<User> AuthenticateTask(string emailAddress, string passwordHash) =>
            _users.Find(user => user.EmailAddress.Equals(emailAddress) && user.PasswordHash.Equals(passwordHash))
                .FirstOrDefaultAsync();

        public async void CreateUserAsync(User user) => await _users.InsertOneAsync(user);
    }

}
