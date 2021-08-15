using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace rpcserver
{
    class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            string queueName = "fila_hello";
            using var connection = factory.CreateConnection();
            using var channel = CreateChannel(connection, queueName);
            
            for(int index = 0; index < 10; index++)
            {
                QueueConsumer.Consumer(channel, queueName, "Consumidor " + index.ToString());
            }
            // {
            //     channel.QueueDeclare(queue: "fila_helo", durable: false, exclusive: false, autoDelete: false, arguments: null);

            //     Console.WriteLine(" [*] Waiting for messages.");

            //     var consumer = new EventingBasicConsumer(channel);
            //     consumer.Received += (model, ea) =>
            //     {
            //         var body = ea.Body.ToArray();
            //         var message = Encoding.UTF8.GetString(body);
            //         Console.WriteLine(" [x] Received {0}", message);
            //     };
            //     channel.BasicConsume(queue: "fila_helo", autoAck: true, consumer: consumer);

            // }
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
            // channel1.Close();
            // connection.Close();
        }

        public static IModel CreateChannel(IConnection connection, string queueName)
        {
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName, 
                durable: false, 
                exclusive: false, 
                autoDelete: false, 
                arguments: null);

            return channel;
        }

    }
}
