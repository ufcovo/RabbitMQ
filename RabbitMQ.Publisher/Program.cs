using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://pmltsnbj:U2ZypivPsw_hV_F1nYD2U4IJ4QRNLgSd@shark.rmq.cloudamqp.com/pmltsnbj");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare("logs-fanout", durable:true, type: ExchangeType.Fanout);

            Enumerable.Range(1, 50).ToList().ForEach(r =>
            {
                string message = $"log {r}";
                var messageBody = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("logs-fanout", "", null, messageBody);

                Console.WriteLine($"Log is sended: {message}");
            });
            
            Console.ReadLine();
        }
    }
}