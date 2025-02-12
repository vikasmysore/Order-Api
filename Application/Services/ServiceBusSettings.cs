namespace Application.Settings
{
    public class ServiceBusSettings
    {
        public required string ConnectionString { get; set; }

        public required string QueueName { get; set; }
    }
}
