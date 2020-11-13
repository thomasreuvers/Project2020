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
        public string Name { get; set; }
        public decimal Weight { get; set; }
        public string Sets { get; set; }
        public string Reps { get; set; }
        public string Description { get; set; }

        // TODO: can Reps and Sets be strings? For more dynamic input?
    }
}
