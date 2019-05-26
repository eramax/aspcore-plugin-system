using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IHostingEnvironment _env;
        private Config _conf { get; set; }
        public IEmoContext Context => new EmoContext(this.ContextOptions);
        public string ConnectionString => _conf?.ConnectionString;
        public List<PluginDescriptor> Plugins => _conf?.Plugins;
        public bool SystemInstalled => _conf.NotNull() ? _conf.SystemInstalled : false;
        public string RootDirectory { get; private set; }
        public string WwwRootDirectory { get; private set; }
        public string PluginsDirectory { get; private set; }

        public DbContextOptions<PluginContext> ContextOptions => 
            new DbContextOptionsBuilder<PluginContext>().UseSqlServer(ConnectionString).Options;

        public Engine(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            LoadConfigs();
        }

        void LoadConfigs()
        {
            Func<Config> func = () => _configuration.Get<Config>();
            _conf = func.Retry(x => x.ConnectionString != null, 100);

            RootDirectory = _env.ContentRootPath;
            WwwRootDirectory = _env.WebRootPath;
            PluginsDirectory = WwwRootDirectory + @"\plugins\";
        }

        public void LoadnSaveConfigs(Config conf)
        {
            _conf = conf;
            conf.SaveToFile("Config.json");

            RootDirectory = _env.ContentRootPath;
            WwwRootDirectory = _env.WebRootPath;
            PluginsDirectory = WwwRootDirectory + @"\plugins\";
        }

        public T Resolve<T>() where T : class
        {
            return null;
        }
    }
}
