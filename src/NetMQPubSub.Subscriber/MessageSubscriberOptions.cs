namespace NetMQPubSub.Subscriber;

using NetMQ;

public class MessageSubscriberOptions : SocketOptions
{
	public MessageSubscriberOptions(MessageSubscriber publisher)
		: base(publisher.socket)
	{
	}
}
