using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Engines;
using SharedKernel.IServices;

namespace InstallerWebApp.Controllers
{
    public class PluginController : Controller
    {
        private readonly IEngine _engine;
        private readonly IPluginService _service;

        public PluginController(IEngine engine, IPluginService service)
        {
            _engine = engine;
            _service = service;
        }
        public IActionResult Index()
        {
            return Ok(_engine.Plugins.ToJson());
        }

        public IActionResult Install1()
        {
            var plugin1 = @"C:\Users\eramax\source\repos\InstallerWebApp\Plugins\PaypalPaymentPlugin\bin\Debug\netcoreapp2.2\netcoreapp2.2.zip";
            _service.InstallPlugin(plugin1);
            return RedirectToAction("Index", "PaypalPay");
        }

        public IActionResult Load()
        {
            var plugin1 = new string[]
            {
                @"C:\Users\eramax\source\repos\InstallerWebApp\Plugins\PaypalPaymentPlugin\bin\Debug\netcoreapp2.2\PaypalPaymentPlugin.dll"
            };
            _engine.LoadAssembly(plugin1);
            return RedirectToAction("Index", "PaypalPay");
        }
    }
}