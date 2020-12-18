using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Project.Entities;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    [Authorize]
    public class SchematicsController : Controller
    {
        private readonly SchematicService _schemaService;

        public SchematicsController(SchematicService schemaService)
        {
            _schemaService = schemaService;
        }

        // panel/schematics
        [HttpGet("panel/{controller}")]
        public async Task<IActionResult> Index()
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

        // panel/schematics/create
        [HttpGet("panel/{controller}/create")]
        public async Task<IActionResult> CreateSchematicAsync()
        {
            return View("CreateSchematic");
        }

        // panel/schematics/create
        [HttpPost("panel/{controller}/create")]
        public async Task<IActionResult> CreateSchematicAsync(SchematicModel model)
        {
            // Check if model state is valid
            if (!ModelState.IsValid) return View("CreateSchematic");

            // Check if exercise contains any items and if it's not null
            if (model.Exercises.Contains(null) || model.Exercises.Count <= 0)
                return RedirectToAction("CreateSchematicAsync");

            await _schemaService.CreateSchemaAsync(new Schematic
            {
                Name = model.SchemaName,
                TimeOfCreation = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                Exercises = model.Exercises

            });

            return RedirectToAction("Index");
        }

    }
}

//TODO: Implement exception handling
