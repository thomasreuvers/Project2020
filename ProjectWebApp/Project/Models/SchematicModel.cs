using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Project.Entities;

namespace Project.Models
{
    public class SchematicModel
    {
        public string Id { get; set; }

        public string ActionType { get; set; }

        [Required]
        public string SchemaName { get; set; }

        [Required]
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
