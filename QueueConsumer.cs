using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace rpcserver
{
    public static class QueueConsumer
    {

        public static void Consume(IModel channel)
        {
            channel.QueueDeclare(queue: "fila_helo", durable: false, exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                    channel.BasicAck(ea.DeliveryTag,false);
                }
                catch (Exception ex)
                {
                    channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };
            channel.BasicConsume(queue: "fila_helo", autoAck: true, consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
