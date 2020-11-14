using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Project.Entities;

namespace Project.Services
{
    public class SchematicService
    {
        private readonly IMongoCollection<Schematic> _schemas;

        public SchematicService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _schemas = database.GetCollection<Schematic>(settings.SchematicCollectionName);
        }

        public Task<List<Schematic>> Get() => _schemas.Find(schema => true).ToListAsync();
        public async Task<Schematic> CreateSchemaAsync(Schematic schema)
        {
            await _schemas.InsertOneAsync(schema);
            return schema;
        }

        public async void UpdateSchemaAsync(Schematic schema) => await _schemas.ReplaceOneAsync(x => x.Id == schema.Id, schema);
    }
}
