using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
                    assembly = Assembly.LoadFrom(file);
                }
                catch (FileLoadException)
                {
                    if (useUnsafeLoadAssembly)
                    {
                        //if an application has been copied from the web, it is flagged by Windows as being a web application,
                        //even if it resides on the local computer.You can change that designation by changing the file properties,
                        //or you can use the<loadFromRemoteSources> element to grant the assembly full trust.As an alternative,
                        //you can use the UnsafeLoadFrom method to load a local assembly that the operating system has flagged as
                        //having been loaded from the web.
                        //see http://go.microsoft.com/fwlink/?LinkId=155569 for more information.
                        assembly = Assembly.UnsafeLoadFrom(file);
                    }
                    else
                        throw;
                }

                //register the plugin definition
                _partManager.ApplicationParts.Add(new AssemblyPart(assembly));
            }

            return true;
        }
    }
}
