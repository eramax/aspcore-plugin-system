using System.Collections.Generic;

namespace SharedKernel.Plugins
{
    public class PluginInfo
    {
        public string Name { get; set; }
        public List<string> Dependencies { get; set; }
        public double Version { get; set; }
        public double SupportedVersion { get; set; }
        public string Desctiption { get; set; }
        public string Logo { get; set; }
        public string Author { get; set; }
        public  string MainDll { get; set; }
    }
}
