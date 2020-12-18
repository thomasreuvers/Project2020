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

    }
}
