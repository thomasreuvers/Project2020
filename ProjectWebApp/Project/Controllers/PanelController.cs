using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Project.Entities;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    [Authorize]
    public class PanelController : Controller
    {
        private readonly SchematicService _schemaService;

        public PanelController(SchematicService schemaService)
        {
            _schemaService = schemaService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Schematics(string schematicId = "")
        
        {
            if (string.IsNullOrEmpty(schematicId))
            {
                return View();
            }

            var schematic = await _schemaService.Get(schematicId);

            if (schematic == null) return View();

            var schematicModel = new SchematicModel
            {
                ActionType = "edit",
                Exercises = schematic.Exercises,
                SchemaName = schematic.Name,
                Id = schematicId
            };

            return View(schematicModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSchematic(SchematicModel schematicModel)
        {
            if (!ModelState.IsValid) return RedirectToAction("Schematics");

            // Check if list has any items and if so check if not all items are null
            if (!schematicModel.Exercises.Any() || schematicModel.Exercises.All(x => x == null)) return RedirectToAction("Schematics");

            if (!string.IsNullOrEmpty(schematicModel.Id) && schematicModel.ActionType == "edit")
            {
                _schemaService.UpdateSchemaAsync(new Schematic
                {
                    Exercises = schematicModel.Exercises,
                    Id = schematicModel.Id,
                    Name = schematicModel.SchemaName
                });
            }
            else
            {
                await _schemaService.CreateSchemaAsync(new Schematic
                {
                    Exercises = schematicModel.Exercises,
                    Name = schematicModel.SchemaName
                });
            }

            return RedirectToAction("Schematics");
        }

    }
}
