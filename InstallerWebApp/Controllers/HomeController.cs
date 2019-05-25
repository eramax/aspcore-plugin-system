using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using InstallerWebApp.Models;
using SharedKernel.Engines;
using SharedKernel.Data;
using Core.Models;
using System;

namespace InstallerWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEngine _enginee;
        private readonly IRepository<Product> _proRepository;

        public HomeController(IEngine enginee, IRepository<Product> proRepository)
        {
            _enginee = enginee;
            _proRepository = proRepository;
        }
        public IActionResult Index()
        {
            var p1 = new Product { Name = "Box", Type = "Wood", Stock = 100 };
            _proRepository.Insert(p1);

            var x = _proRepository.Table.Where(t => t.Name == "Box").ToList();
            return Ok(x.ToJson());
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
