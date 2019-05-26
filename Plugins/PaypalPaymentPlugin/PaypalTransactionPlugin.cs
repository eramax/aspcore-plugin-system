using Microsoft.Extensions.DependencyInjection;
using PaypalPaymentPlugin.Models;
using PaypalPaymentPlugin.Services;
using SharedKernel.Engines;
using SharedKernel.Plugins;
using System;

namespace PaypalPaymentPlugin
{
    public class PaypalTransactionPlugin : Plugin
    {
        public PaypalTransactionPlugin(IEngine engine, IServiceCollection services) : base(engine, services)
        {
        }

        public override bool Install()
        {
            //---- install Context
            return new PaypalTranscationContext(engine.ContextOptions).InstallContext();
        }

        public override bool Load()
        {
            //services.AddTransient<IPaypalService, PaypalService>();
            return true;
        }

        public override bool Uninstall()
        {
            throw new NotImplementedException();
        }
    }
}
