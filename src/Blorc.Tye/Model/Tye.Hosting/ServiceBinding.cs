namespace Blorc.Tye.Model.Tye.Hosting
{
    public class ServiceBinding
    {
        public string ConnectionString { get; set; }

        public string Host { get; set; }

        public int? InternalPort { get; set; }

        public string Name { get; set; }

        public int? Port { get; set; }

        public string Protocol { get; set; }
    }
}
