using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Project.Entities;

namespace Project.Services
{
    public class SchemaService
    {
        private readonly IMongoCollection<Schema> _schemas;

        public SchemaService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _schemas = database.GetCollection<Schema>(settings.SchemasCollectionName);
        }

        public Task<List<Schema>> Get() => _schemas.Find(schema => true).ToListAsync();
        public async Task<Schema> CreateSchemaAsync(Schema schema)
        {
            await _schemas.InsertOneAsync(schema);
            return schema;
        }

        public async void UpdateSchemaAsync(Schema schema) => await _schemas.ReplaceOneAsync(x => x.Id == schema.Id, schema);
    }
}
