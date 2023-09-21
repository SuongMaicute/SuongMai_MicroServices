namespace Suongmai.Services.OrderAPI.RabbitMQSender
{
    public interface IRabbbitIMOrderMessageSender
    {
        void SendMessage (Object message, string exchangeName);
    }
}
