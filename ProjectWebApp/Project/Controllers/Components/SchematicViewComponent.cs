using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services;

namespace Project.Controllers.Components
{
    public class SchematicViewComponent : ViewComponent
    {
        private readonly SchematicService _schemaService;

        public SchematicViewComponent(SchematicService schemaService)
        {
            _schemaService = schemaService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var schematics = await _schemaService.Get();

            var schematicModels = schematics.Select(schematic => new SchematicModel
            {
                SchemaName = schematic.Name,
                Exercises = schematic.Exercises,
                Id = schematic.Id
            }).ToList();

            return View(schematicModels);
        }
    }
}
