using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Data;
using SharedKernel.Models;
using SharedKernel.Plugins;

namespace SharedKernel.Engines
{
    public interface IEngine
    {
        bool SystemInstalled { get; }
        IEmoContext Context { get; }
        string ConnectionString { get;  }
        List<PluginDescriptor> Plugins { get; }
        void LoadnSaveConfigs(Config conf);
        DbContextOptions<PluginContext> ContextOptions { get; }
        string RootDirectory { get; }
        string WwwRootDirectory { get; }
        string PluginsDirectory { get; }
        bool LoadAssembly(string[] assemblyFiles, bool useUnsafeLoadAssembly = false);
    }
}