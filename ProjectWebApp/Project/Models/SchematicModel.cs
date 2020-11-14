using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class SchematicModel
    {
        public string SchemaName { get; set; }

        public List<string> InputFields { get; set; }

        /* TODO:
         *  Props:
         *  - CreatedBy string User.Username
         *  - CreatedDate DateTime DateTime.now
         *  - etc?
         *
         */ 
    }
}
