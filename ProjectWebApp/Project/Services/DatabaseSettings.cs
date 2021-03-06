﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Services
{
    public class DatabaseSettings : IDatabaseSettings
    {
       public string UsersCollectionName { get; set; }
       public string SchematicCollectionName { get; set; }
       public string ConnectionString { get; set; }
       public string DatabaseName { get; set; }
    }

    public interface IDatabaseSettings
    {
        string UsersCollectionName { get; set; }
        string SchematicCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
