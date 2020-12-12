using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class SchematicModel
    {
        [Required]
        public string SchemaName { get; set; }

        [Required]
        public List<string> InputFields { get; set; } = new List<string>();

        /* TODO:
         *  Props:
         *  - CreatedBy string User.Username
         *  - CreatedDate DateTime DateTime.now
         *  - etc?
         *
         */ 
    }
}
