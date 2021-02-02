namespace Blorc.Tye
{
    using System.Collections.Generic;

    using YamlDotNet.Serialization;

    public class ServiceDescription
    {
        public string Args { get; set; }

        public List<ServiceBinding> Bindings { get; set; } = new List<ServiceBinding>();

        public bool? Build { get; set; } = true;

        [YamlMember(Alias = "env")]
        public List<ConfigurationSource> Configuration { get; set; } = new List<ConfigurationSource>();

        public string DockerImage { get; set; }

        public string Executable { get; set; }

        public bool External { get; set; }

        public string Name { get; set; }

        public string Project { get; set; }

        public int? Replicas { get; set; }

        public string WorkingDirectory { get; set; }
    }
}
