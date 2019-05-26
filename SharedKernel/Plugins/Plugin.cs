using System;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Engines;

namespace SharedKernel.Plugins
{
    public abstract class Plugin : IPlugin
    {
        protected readonly IEngine engine;

        public Plugin(IEngine engine)
        {
            this.engine = engine;
        }

        public abstract bool Install();
        public abstract bool Uninstall();
        public abstract bool Load();
    }
}
