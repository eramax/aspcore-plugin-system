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
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Data;
using SharedKernel.Engines;
using SharedKernel.Models;
using SharedKernel.Plugins;

namespace Core
{
    public class Engine : IEngine
    {
        public Engine(IConfiguration configuration, IHostingEnvironment env,
            ApplicationPartManager partManager,
            IServiceCollection ServiceCollection)
        {
            _configuration = configuration;
            _env = env;
            _partManager = partManager;
            this.ServiceCollection = ServiceCollection;
            LoadConfigs();
        }

        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;
        private readonly ApplicationPartManager _partManager;
        public IServiceCollection ServiceCollection { get; }
        private IServiceProvider ServiceProvider;
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


        private void LoadConfigs()
        {
            Func<Config> func = () => _configuration.Get<Config>();
            _conf = func.Retry(x => x.ConnectionString != null, 100);

            RootDirectory = _env.ContentRootPath;
            WwwRootDirectory = _env.WebRootPath;
            PluginsDirectory = WwwRootDirectory + @"\plugins\";
        }

        private void LoadnSaveConfigs(Config conf)
        {
            _conf = conf;
            conf.SaveToFile("Config.json");

            RootDirectory = _env.ContentRootPath;
            WwwRootDirectory = _env.WebRootPath;
            PluginsDirectory = WwwRootDirectory + @"\plugins\";
        }

        public void SetConnectionString(string connection)
        {
            _conf.ConnectionString = connection;
            _conf.SystemInstalled = true;
            SaveConfig();
        }

        public void AddPluginConfig(PluginDescriptor plugin)
        {
            //for testing i will reset plugin list 
            _conf.Plugins = null;
            if (_conf.Plugins == null )_conf.Plugins = new List<PluginDescriptor>();
            _conf.Plugins.Add(plugin);
            SaveConfig();
        }

        private void SaveConfig()
        {
            _conf.SaveToFile("Config.json");
        }
        public T Resolve<T>() where T : class
        {
            return ServiceProvider.GetService<T>() as T;
        }
        public T Resolve<T>(Type type) where T : class
        {
            return ServiceProvider.GetService(type) as T;
        }
        public void RegisterType(Type type)
        {
            ServiceCollection.AddTransient(type);
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        public void RegisterSignleTone<IT, TC>() where  IT : class where TC: class , IT
        {
            ServiceCollection.AddSingleton<IT,TC>();
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }
        public void RegisterTransient<IT, TC>() where IT : class where TC : class, IT
        {
            ServiceCollection.AddTransient<IT, TC>();
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }
        public void RegisterScoped<IT, TC>() where IT : class where TC : class, IT
        {
            ServiceCollection.AddScoped<IT, TC>();
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }
        /// <summary>
        /// Load and register the assembly
        /// </summary>
        /// <param name="applicationPartManager">Application part manager</param>
        /// <param name="assemblyFile">Path to the assembly file</param>
        /// <param name="useUnsafeLoadAssembly">Indicating whether to load an assembly into the load-from context, bypassing some security checks</param>
        /// <returns>Assembly</returns>
        public List<Assembly> LoadAssembly(params string[] assemblyFiles)
        {
            //try to load a assembly
            List<Assembly> assemblys = new List<Assembly>();
            Assembly assembly;
            foreach (var file in assemblyFiles)
            {
                try
                {
                    assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                    if (assembly != null)
                    {
                        //register the plugin definition
                        _partManager.ApplicationParts.Add(new AssemblyPart(assembly));
                        // Notify change
                        MyActionDescriptorChangeProvider.Instance.HasChanged = true;
                        MyActionDescriptorChangeProvider.Instance.TokenSource.Cancel();
                        assemblys.Add(assembly);
                    }
                }
                catch (FileLoadException)
                {
                }
            }
            return assemblys;
        }
    }
}
