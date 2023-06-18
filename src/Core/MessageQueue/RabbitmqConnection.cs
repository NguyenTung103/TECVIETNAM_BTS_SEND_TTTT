namespace Core.MessageQueue
{
    public class MasterRabbitmqConnection
    {
        public string HostName { get; set; }

        public int Port { get; set; } = 5672;

        public string UserName { get; set; }

        public string Password { get; set; }
        public string WorkerQueueName { get; set; }

        public string VirtualHost { get; set; } = "/";

        public int ContinuationTimeout { get; set; } = 10;
    }

    public class WorkerRabbitmqConnection : MasterRabbitmqConnection
    {

    }
}
