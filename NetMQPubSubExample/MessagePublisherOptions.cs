namespace NetMQPubSubExample;

using NetMQ;

public class MessagePublisherOptions : SocketOptions
{
	public MessagePublisherOptions(MessagePublisher publisher)
		: base(publisher.socket)
	{
	}
}
