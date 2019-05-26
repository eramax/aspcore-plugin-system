using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using SharedKernel.Engines;
using SharedKernel.Helpers;
using SharedKernel.IServices;
using SharedKernel.Plugins;

namespace Core.Services.Plugins
{
    public class PluginService : IPluginService
    {
        private readonly IEngine _engine;

        public PluginService(IEngine engine)
        {
            _engine = engine;
        }

        public IEnumerable<Plugin> LoadPlugins()
        {
            // get list of plugins from engine
            var pluginlist = _engine.Plugins;
            foreach (var pg in pluginlist)
            {
                LoadPlugin(pg);
            }
            return null;
        }


        public Plugin LoadPlugin(PluginDescriptor plugin)
        {
            // load dll files for each plugin
            // loop on the plugin list and execute load methond on each plugin such register services

            return null;
        }

        public bool InstallPlugin(string archivefile)
        {
            var plugindir = _engine.PluginsDirectory.AppendDir(new Guid().New(12));
            FileManager.ExtractZipToDirectory(archivefile, plugindir);
            var dllFiles = FileManager.GetFiles(plugindir);
            _engine.LoadAssembly(dllFiles);
            // extract file to plugins folder
            // install context
            // 
            // update plugin list and save config and reload configs

            return true;
        }

        public bool UninstallPlugin(Plugin plugin)
        {
            // select plugin from the plugin list in engine
            // call uninstall on this plugin
            return true;
        }

        
    }
}
