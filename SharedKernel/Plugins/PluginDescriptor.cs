using System.Collections.Generic;

namespace SharedKernel.Plugins
{
    public class PluginDescriptor
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool Installed { get; set; }
        public string MainDll { get; set; }
        public List<string> OtherDlls { get; set; }
        public double Version { get; set; }
        public double SupportedVersion { get; set; }
        public string Desctiption { get; set; }
        public string Logo { get; set; }
        public string Author { get; set; }
        public bool Uninstallable { get; set; }
        public List<string> Dependencies { get; set; }
    }
}
