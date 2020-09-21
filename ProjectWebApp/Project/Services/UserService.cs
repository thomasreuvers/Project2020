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

        public async Task<User> GetUserByEmailAsync(string emailAddress) => 
            await _users.Find(user => user.EmailAddress.Equals(emailAddress)).FirstOrDefaultAsync();

        public async Task<User> GetUserByIdAsync(string id) => await _users.Find(user => user.Id.Equals(id)).FirstOrDefaultAsync();

        public async Task<User> CreateUserAsync(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async void UpdateUserAsync(User user) => await _users.ReplaceOneAsync(x => x.Id == user.Id, user);

    }

}
