using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://pmltsnbj:U2ZypivPsw_hV_F1nYD2U4IJ4QRNLgSd@shark.rmq.cloudamqp.com/pmltsnbj");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            var randomQueueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(randomQueueName, "logs-fanout", "", null);

            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(randomQueueName, false, consumer);
            Console.WriteLine("Logs are loading..");

            consumer.Received += (object? sender, BasicDeliverEventArgs e) => {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1000);
                Console.WriteLine("Coming message: " + message);
                channel.BasicAck(e.DeliveryTag, false);
            };


            
            Console.ReadLine();
        }
    }
}