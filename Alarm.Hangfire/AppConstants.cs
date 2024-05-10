namespace Alarm.Hangfire;

public static class AppConstants
{
    public static class RabbitMQConstants
    {
        public const string ConnectionSettings = "RabbitMQSettings:Default";
        public const string HangfireQueueName = "Hangfire";
        public const string WriteToDBExchange = "WriteToDB";
        public const string WriteToDBRoutingKey = "b232848c-80b7-48b5-93db-4e170689b2cb";
    }

    public static class HangfireConstants
    {
        public const string DbConnection = "HangfireConnection";
    }
}
