using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Entities
{
    public class Exercise
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Weight { get; set; }
        public string Sets { get; set; }
        public string Reps { get; set; }

        // TODO: can Reps and Sets be strings? For more dynamic input?
    }
}
