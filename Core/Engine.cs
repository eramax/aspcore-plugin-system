using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedKernel.Data;
using SharedKernel.Engines;
using SharedKernel.Models;
using SharedKernel.Plugins;

namespace Core
{
    public class Engine : IEngine
    {
        private readonly IConfiguration _configuration;

        public IEmoContext Context => new EmoContext(this.ContextOptions);
        public string ConnectionString { get; private set; }
        public List<PluginDescriptor> Plugins { get; private set; }
        public bool SystemInstalled { get; private set; }

        public DbContextOptions<PluginContext> ContextOptions => 
            new DbContextOptionsBuilder<PluginContext>().UseSqlServer(ConnectionString).Options;

        public Engine(IConfiguration configuration)
        {
            _configuration = configuration;
            Reload();
        }

        public void Reload()
        {
            Func<Config> func = () => _configuration.Get<Config>();
            Config conf = func.Retry(x => x.ConnectionString != null, 100);

            Plugins = conf.Plugins;
            ConnectionString = conf.ConnectionString;
            SystemInstalled = conf.SystemInstalled;
        }

        public T Resolve<T>() where T : class
        {
            return null;
        }
    }
}
