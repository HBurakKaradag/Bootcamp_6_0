using System.Text;
using System.Security.AccessControl;
using System;
using Hotels.API.Entities;
using RabbitMQ.Client;
using Hotels.API.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Hotels.API.Publishers
{
    public class RoomPublisher : IRoomPublisher
    {
        private IConnection _connection;
        private readonly RabbitConfiguration _rabbitConfig;

        public RoomPublisher(IOptions<RabbitConfiguration> rabbitConfig)
        {
            _rabbitConfig = rabbitConfig.Value;
        }
        public void PublishRoomAdd(RoomEntity entity)
        {

            if(CheckConnection())
            {
                using(var connectionChannel = _connection.CreateModel())
                {
                    var jsonValue = JsonConvert.SerializeObject(entity);
                    var messageBody = Encoding.UTF8.GetBytes(jsonValue);
                    connectionChannel.BasicPublish(
                        exchange: _rabbitConfig.Exchange,
                        routingKey : "AddRoomEvent",
                        body : messageBody
                    );
                }
            }
        }


        private void CreateConnection()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = _rabbitConfig.Host,
                UserName = _rabbitConfig.UserName,
                Password = _rabbitConfig.Password
            };

            _connection = connectionFactory.CreateConnection();
        }

        private bool CheckConnection()
        {
            if(_connection != null)
                return true;
            
            CreateConnection();
            return _connection != null;
        }


    }
}
