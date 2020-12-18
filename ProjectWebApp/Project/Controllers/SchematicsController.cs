using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
                TimeOfCreation = schematic.TimeOfCreation,
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

        // panel/schematics/edit/{id}
        [HttpGet("panel/{controller}/edit/{id}", Name = "editSchematicRoute")]
        public async Task<IActionResult> EditSchematicAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            var schematic = await _schemaService.Get(id);

            if (schematic == null) return RedirectToAction("Index");

            var schematicModel = new SchematicModel
            {
                Exercises = schematic.Exercises,
                Id = schematic.Id,
                SchemaName = schematic.Name
            };


            return View("EditSchematic", schematicModel);
        }

        // panel/schematics/edit/
        [HttpPost("panel/{controller}/edit/{id}", Name = "editSchematicRoute")]
        public async Task<IActionResult> EditSchematicAsync(SchematicModel model)
        {
            // Check if model state is valid
            if (!ModelState.IsValid) return View("EditSchematic");

            // Check if exercise contains any items and if it's not null
            if (model.Exercises.Contains(null) || model.Exercises.Count <= 0)
                return RedirectToAction("EditSchematicAsync");

            if (string.IsNullOrEmpty(model.Id)) return RedirectToAction("EditSchematicAsync");

            _schemaService.UpdateSchemaAsync(new Schematic
            {
                Id = model.Id,
                Name = model.SchemaName,
                Exercises = model.Exercises
            });

            return RedirectToAction("Index");
        }

        // panel/schematics/delete{id}
        [Route("panel/{controller}/delete/{id}", Name = "deleteSchematicRoute")]
        public async Task<IActionResult> DeleteSchematicAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            var schematic = _schemaService.Get(id);

            if (schematic == null) return RedirectToAction("Index");
            
            _schemaService.DeleteSchematicAsync(id);

            return RedirectToAction("Index");
        }
    }
}

//TODO: Implement exception handling
