using Microsoft.AspNetCore.Mvc;
using PaypalPaymentPlugin.Services;
using SharedKernel.Engines;
using System;

namespace PaypalPaymentPlugin.Controllers
{
    public class PaypalPayController : Controller
    {
        
        //private readonly IEngine _engine;
        //private readonly IPaypalService _service;
        public PaypalPayController()
        {
           // _engine = engine;
            //_service = service;
        }

        public IActionResult Index()
        {
            return Ok("Hello Form Plugin");
            //return Ok(_service.Test().ToJson());
        }

        public IActionResult Plugins()
        {
            return Ok("Hello Form Plugin");
            //return Ok(_engine.Plugins.ToJson());
        }

    }
}
