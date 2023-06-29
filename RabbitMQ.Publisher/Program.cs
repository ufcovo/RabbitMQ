using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ.Publisher
{
    public enum LogNames
    {
        Critical = 1, Error = 2, Warning = 3, Info = 4
    }
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://pmltsnbj:U2ZypivPsw_hV_F1nYD2U4IJ4QRNLgSd@shark.rmq.cloudamqp.com/pmltsnbj");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

            Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
            {
                var routeKey = $"route-{x}";

                var queueName = $"direct-queue-{x}";
                channel.QueueDeclare(queueName, true, false, false);
                channel.QueueBind(queueName, "logs-direct", routeKey, null);
            });

            Enumerable.Range(1, 50).ToList().ForEach(r =>
            {
                LogNames logName = (LogNames)new Random().Next(1, 5);
                string message = $"log-type: {logName}";
                var messageBody = Encoding.UTF8.GetBytes(message);

                var routeKey = $"route-{logName}";
                channel.BasicPublish("logs-direct", routeKey, null, messageBody);

                Console.WriteLine($"Log is sended: {message}");
            });

            Console.ReadLine();
        }
    }
}