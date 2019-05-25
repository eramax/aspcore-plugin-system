using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Data;
using SharedKernel.Plugins;

namespace SharedKernel.Engines
{
    public interface IEngine
    {
        bool SystemInstalled { get; }
        IEmoContext Context { get; }
        string ConnectionString { get;  }
        List<PluginDescriptor> Plugins { get; }
        void Reload();
        DbContextOptions<PluginContext> ContextOptions { get; }
    }
}