using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Project.Entities;
using Project.Models;
using Project.Services;

namespace Project.Controllers
{
    [Authorize]
    public class PanelController : Controller
    {
        private readonly List<Exercise> _exercises = new List<Exercise>();
        private readonly SchematicService _schemaService;

        public PanelController(SchematicService schemaService)
        {
            _schemaService = schemaService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Schematics()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Schematics(SchematicModel schematicModel)
        {
            if (!ModelState.IsValid) return View();

            // Check if list has any items and if so check if not all items are null
            if (!schematicModel.InputFields.Any() || schematicModel.InputFields.All(x => x == null)) return View();

            MapStringListToExercise(schematicModel.InputFields);
            await _schemaService.CreateSchemaAsync(new Schematic
                {
                    Exercises = _exercises,
                    Name = schematicModel.SchemaName
                });

            // Clear modelstate so our previous input will not be saved.
            ModelState.Clear();
            //TODO: make update functionality in the same form

            return View();
        }

        private void MapStringListToExercise(List<string> list)
        {
            if(list == null || !list.Any()) return;

            var propAmount = typeof(Exercise).GetProperties().Length;
            var propData = list.Take(propAmount).ToList();
            list.RemoveRange(0, propAmount);

            var exercise = new Exercise();

            foreach (var prop in exercise.GetType().GetProperties())
            {
                if(propData.Count.Equals(0)) break;

                switch (Type.GetTypeCode(prop.PropertyType))
                {
                    case TypeCode.Decimal:
                        if(!decimal.TryParse(propData.ElementAt(0), out var result)) continue;
                        prop.SetValue(exercise, result, null);
                        break;
                    default:
                        prop.SetValue(exercise, propData.ElementAt(0), null);
                        break;
                }

                propData.RemoveAt(0);
            }

            _exercises.Add(exercise);
            MapStringListToExercise(list);
        }
    }
}
