﻿using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using SharedKernel.Plugins;

namespace SharedKernel.IServices
{
    public interface IPluginService
    {
        IEnumerable<Plugin> LoadPlugins();
        bool InstallPlugin(string file);
        bool UninstallPlugin(Plugin plugin);
    }
}
