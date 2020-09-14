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

        public Task<User> AuthenticateTask(string emailAddress, string passwordHash) =>
            _users.Find(user => user.EmailAddress.Equals(emailAddress) && user.PasswordHash.Equals(passwordHash))
                .FirstOrDefaultAsync();
    }

}
