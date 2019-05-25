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

        public IEmoContext Context
        {
            get
            {
                var optionsBuilder = new DbContextOptionsBuilder<EmoContext>();
                optionsBuilder.UseSqlServer(this.ConnectionString);
                return new EmoContext(optionsBuilder.Options);
            }
        }

        public string ConnectionString { get; private set; }
        public List<PluginDescriptor> Plugins { get; private set; }
        public bool SystemInstalled { get; private set; }


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
