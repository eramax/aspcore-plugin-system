using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using Core.Services.Plugins;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
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
        private readonly ApplicationPartManager _partManager;
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

        public Engine(IConfiguration configuration, IHostingEnvironment env, ApplicationPartManager partManager)
        {
            _configuration = configuration;
            _env = env;
            _partManager = partManager;
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

        /// <summary>
        /// Load and register the assembly
        /// </summary>
        /// <param name="applicationPartManager">Application part manager</param>
        /// <param name="assemblyFile">Path to the assembly file</param>
        /// <param name="useUnsafeLoadAssembly">Indicating whether to load an assembly into the load-from context, bypassing some security checks</param>
        /// <returns>Assembly</returns>
        public bool LoadAssembly(string[] assemblyFiles, bool useUnsafeLoadAssembly = false)
        {
            //try to load a assembly
            Assembly assembly;
            foreach (var file in assemblyFiles)
            {
                try
                {
                    assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                    if (assembly != null)
                    {
                        _partManager.ApplicationParts.Add(new AssemblyPart(assembly));
                        // Notify change
                        MyActionDescriptorChangeProvider.Instance.HasChanged = true;
                        MyActionDescriptorChangeProvider.Instance.TokenSource.Cancel();
                    }
                }
                catch (FileLoadException)
                {
                }

                //register the plugin definition
                //_partManager.ApplicationParts.Add(new AssemblyPart(assembly));
            }

            return true;
        }
    }
}
