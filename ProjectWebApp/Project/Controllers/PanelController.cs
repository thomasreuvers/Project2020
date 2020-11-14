using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index()
        {
            var schemas = await _schemaService.Get();

            var schemaModel = new SchematicModel {InputFields = new List<string>()};

            foreach (var schema in schemas.TakeWhile(schema => schemas.Count != 0))
            {
                foreach (var exercise in schema.Exercises)
                {
                    foreach (var prop in exercise.GetType().GetProperties())
                    {
                        schemaModel.InputFields.Add(prop.GetValue(exercise).ToString());

                    }
                }

                schemaModel.SchemaName = schema.Name;
            }

            return View(schemaModel);
        }

        public async Task<IActionResult> UploadTask(SchematicModel schemaModel)
        {
            if (schemaModel == null) return View("Index");

            MapObject(schemaModel.InputFields);

            await _schemaService.CreateSchemaAsync(new Schematic
            {
                Name = schemaModel.SchemaName,
                Exercises = _exercises
            });

            /* TODO:
             * Save exercises seperatly and save a array of object id's in schemas collection instead of whole object?
             */

            return View("Index");
        }

        private void MapObject(List<string> list)
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
            MapObject(list);
        }
    }
}
