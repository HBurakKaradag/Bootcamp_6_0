using System.Runtime.InteropServices;
using System.Text;
using System.Reflection.Emit;
using System.Resources;
using System;
using System.Threading;
using System.Threading.Tasks;
using Hotels.API.Consumers.Services;
using Hotels.API.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Hotels.API.Consumers.Workers
{
    public class RoomQueueReceiver : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly RabbitConfiguration _rabbitConfig;
        private readonly QueueRoutes _queueRoutes;
        private readonly IRoomConsumeService _roomConsumeService;

        public RoomQueueReceiver(IOptions<RabbitConfiguration> rabbitConfig,
                                 IOptions<QueueRoutes> queueRoutes,
                                 IRoomConsumeService roomConsumeService)
        {
            _rabbitConfig = rabbitConfig.Value;
            _queueRoutes = queueRoutes.Value;
            _roomConsumeService = roomConsumeService;
            ConnectConsumer();
        }

        private void ConnectConsumer()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = _rabbitConfig.Host,
                UserName = _rabbitConfig.UserName,
                Password = _rabbitConfig.Password
            };

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                queue: _queueRoutes.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                Enum.TryParse(ea.RoutingKey, out RouteKeyEnums routeKeyEnum);
                HandleMessages(routeKeyEnum, content);

                // ShutDown , ConsumeCancelld dalegate consumer.Shutdown += 
               _channel.BasicAck(ea.DeliveryTag,false);
            };

             _channel.BasicConsume(_queueRoutes.QueueName, false, consumer);
            return Task.CompletedTask;
        }

        private async void HandleMessages(RouteKeyEnums routeKey, string content)
        {
            switch (routeKey)
            {
                case RouteKeyEnums.AddRoomEvent:
                    await _roomConsumeService.AddRoomEventConsume(content);
                    break;
                case RouteKeyEnums.DeleteRoomEvent:
                    // await _roomConsumeService.Delete 
                    break;
                default:
                    break;
            }

        }

    }
}
