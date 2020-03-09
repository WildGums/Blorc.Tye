namespace Blorc.Model.Micronetes.Hosting
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reactive.Subjects;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class Service
    {
        public Service()
        {
            this.Logs.Subscribe(
                entry =>
                    {
                        if (this.CachedLogs.Count > 5000)
                        {
                            this.CachedLogs.Dequeue();
                        }

                        this.CachedLogs.Enqueue(entry);
                    });
        }

        [JsonIgnore]
        public Queue<string> CachedLogs { get; } = new Queue<string>();

        public ServiceDescription Description { get; set; }

        [JsonIgnore]
        public Dictionary<object, object> Items { get; } = new Dictionary<object, object>();

        [JsonIgnore]
        public Subject<string> Logs { get; } = new Subject<string>();

        [JsonIgnore]
        public Dictionary<int, List<int>> PortMap { get; set; } = new Dictionary<int, List<int>>();

        [JsonIgnore]
        public Subject<ReplicaEvent> ReplicaEvents { get; } = new Subject<ReplicaEvent>();

        public ConcurrentDictionary<string, ReplicaStatus> Replicas { get; set; } =
            new ConcurrentDictionary<string, ReplicaStatus>();

        public int Restarts { get; set; }

        public ServiceType ServiceType
        {
            get
            {
                if (this.Description.DockerImage != null)
                {
                    return ServiceType.Container;
                }

                if (this.Description.Project != null)
                {
                    return ServiceType.Project;
                }

                return ServiceType.Executable;
            }
        }

        public ServiceStatus Status { get; set; } = new ServiceStatus();
    }

    public readonly struct ReplicaEvent
    {
        public ReplicaState State { get; }

        public ReplicaStatus Replica { get; }

        public ReplicaEvent(ReplicaState state, ReplicaStatus replica)
        {
            this.State = state;
            this.Replica = replica;
        }
    }

    public enum ReplicaState
    {
        Removed,

        Added,

        Started,

        Stopped
    }

    public class ServiceStatus
    {
        public string Args { get; set; }

        public string ExecutablePath { get; set; }

        public string ProjectFilePath { get; set; }

        public string WorkingDirectory { get; set; }
    }

    public class ProcessStatus : ReplicaStatus
    {
        public ProcessStatus(Service service, string name)
            : base(service, name)
        {
        }

        public IDictionary<string, string> Environment { get; set; }

        public int? ExitCode { get; set; }

        public int? Pid { get; set; }
    }

    public class DockerStatus : ReplicaStatus
    {
        public DockerStatus(Service service, string name)
            : base(service, name)
        {
        }

        public string ContainerId { get; set; }

        public string DockerCommand { get; set; }

        public int DockerLogsPid { get; set; }
    }

    public class ReplicaStatus
    {
        public static JsonConverter<ReplicaStatus> JsonConverter = new Converter();

        public ReplicaStatus(Service service, string name)
        {
            this.Service = service;
            this.Name = name;
        }

        [JsonIgnore]
        public Dictionary<object, object> Items { get; } = new Dictionary<object, object>();

        [JsonIgnore]
        public Dictionary<string, string> Metrics { get; set; } = new Dictionary<string, string>();

        public string Name { get; }

        public IEnumerable<int> Ports { get; set; }

        [JsonIgnore]
        public Service Service { get; }

        private class Converter : JsonConverter<ReplicaStatus>
        {
            public override ReplicaStatus Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, ReplicaStatus value, JsonSerializerOptions options)
            {
                // Use the runtime type since we really want to serialize either the DockerStatus or ProcessStatus
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
            }
        }
    }

    public enum ServiceType
    {
        Project,

        Executable,

        Container
    }

    public class PortMapping
    {
        public int ExternalPort { get; set; }

        public List<int> InteralPorts { get; set; } = new List<int>();
    }
}
