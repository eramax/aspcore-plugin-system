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
        public PaypalTransactionPlugin(IEngine engine) : base(engine)
        {
        }

        public override bool Install()
        {
            //---- install Context
            new PaypalTranscationContext(engine.ContextOptions).InstallContext();
            Load();
            return true;
        }

        public override bool Load()
        {
            //engine.ServiceCollection.AddSingleton<IPaypalService, PaypalService>();
            engine.RegisterSignleTone<IPaypalService, PaypalService>();
            return true;
        }

        public override bool Uninstall()
        {
            throw new NotImplementedException();
        }
    }
}
