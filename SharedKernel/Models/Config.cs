using System.Collections.Generic;
using SharedKernel.Plugins;

namespace SharedKernel.Models
{
    public class Config
    {
        public string ConnectionString { get; set; }
        public List<PluginDescriptor> Plugins { get; set; }
        public bool SystemInstalled { get; set; }

    }
}
