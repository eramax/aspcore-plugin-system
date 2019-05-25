using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Engines;

namespace SharedKernel.Plugins
{
    public abstract class Plugin : IPlugin
    {
        protected readonly IEngine engine;
        protected readonly IServiceCollection services;

        public Plugin(IEngine engine, IServiceCollection services)
        {
            this.engine = engine;
            this.services = services;
        }
        public abstract bool Install();
        public abstract bool Uninstall();
        public abstract bool Load();
    }
}
