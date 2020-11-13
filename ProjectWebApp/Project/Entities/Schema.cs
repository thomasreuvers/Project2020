﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.Entities
{
    public class Schema
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }

        public List<Exercise> Exercises { get; set; }

        /* TODO:
         *  Props:
         *  - CreatedBy string User.Username
         *  - CreatedDate DateTime DateTime.now
         *  - etc?
         *
         */
    }
}
