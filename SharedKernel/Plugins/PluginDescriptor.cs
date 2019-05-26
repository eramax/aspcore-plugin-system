using System.Collections.Generic;

namespace SharedKernel.Plugins
{
    public class PluginDescriptor
    {
        public bool Active { get; set; }
        public bool Installed { get; set; }
        public string MainDll { get; set; }
        public List<string> OtherDlls { get; set; }
        public bool Uninstallable { get; set; }
        public PluginInfo PluginInfo { get; set; }
    }
}
