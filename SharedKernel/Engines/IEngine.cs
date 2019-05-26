using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Data;
using SharedKernel.Models;
using SharedKernel.Plugins;

namespace SharedKernel.Engines
{
    public interface IEngine
    {
        IServiceCollection ServiceCollection { get; }
        bool SystemInstalled { get; }
        IEmoContext Context { get; }
        string ConnectionString { get;  }
        List<PluginDescriptor> Plugins { get; }
        void SetConnectionString(string connection);
        void AddPluginConfig(PluginDescriptor plugin);
        DbContextOptions<PluginContext> ContextOptions { get; }
        string RootDirectory { get; }
        string WwwRootDirectory { get; }
        string PluginsDirectory { get; }
        List<Assembly> LoadAssembly(params string[] assemblyFiles);
        void RegisterType(Type type);
        T Resolve<T>() where T : class;
        T Resolve<T>(Type type) where T : class;
        void RegisterSignleTone<IT, TC>() where IT : class where TC : class, IT;
        void RegisterTransient<IT, TC>() where IT : class where TC : class, IT;
        void RegisterScoped<IT, TC>() where IT : class where TC : class, IT;
    }
}