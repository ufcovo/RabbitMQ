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



            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            var queueName = channel.QueueDeclare().QueueName;
            var routeKey = "*.*.Warning";
            channel.QueueBind(queueName, "logs-topic", routeKey);

            channel.BasicConsume(queueName, false, consumer);
            Console.WriteLine("Logs are loading..");

            consumer.Received += (object? sender, BasicDeliverEventArgs e) => {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1000);
                Console.WriteLine("Coming message: " + message);
                //File.AppendAllText("log-critical.txt", message + "\n");
                channel.BasicAck(e.DeliveryTag, false);
            };


            
            Console.ReadLine();
        }
    }
}