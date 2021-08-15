using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace rpcserver
{
    public static class QueueConsumer
    {

        public static void Consumer(IModel channel, string queueName, string consumidor)
        {
            Task.Run(() => 
            {
                // channel.QueueDeclare(
                //     queue: queueName, 
                //     durable: false, 
                //     exclusive: false, 
                //     autoDelete: false, 
                //     arguments: null);

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($" {consumidor} :  - Msg Recebida : {message} ");
                        channel.BasicAck(ea.DeliveryTag,false);
                    }
                    catch (Exception ex)
                    {
                        channel.BasicNack(ea.DeliveryTag, false, true);
                    }
                };
                channel.BasicConsume(
                    queue: queueName, 
                    autoAck: false, 
                    consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            });
        }

    }
}
