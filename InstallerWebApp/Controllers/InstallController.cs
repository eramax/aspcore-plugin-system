using Microsoft.AspNetCore.Mvc;
using SharedKernel.Engines;
using SharedKernel.IServices;

namespace InstallerWebApp.Controllers
{
    public class InstallController : Controller
    {
        private readonly IEngine _engine;
        private readonly IInstallService _installService;

        public InstallController(IEngine engine, IInstallService installService)
        {
            _engine = engine;
            _installService = installService;
        }

        public IActionResult Index()
        {
            _installService.Run();
            return Ok("Done");
        }
    }
}
