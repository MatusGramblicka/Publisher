namespace Messaging;

public interface IMessageProducer
{
    void SendMessage<T>(T message) where T : class;
}