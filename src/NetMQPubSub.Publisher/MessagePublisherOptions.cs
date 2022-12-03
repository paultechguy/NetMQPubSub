namespace NetMQPubSub.Publisher;

using NetMQ;
using NetMQPubSub.Shared.Interfaces;

public class MessagePublisherOptions : SocketOptions, IMessagePublisherOptions
{
	public MessagePublisherOptions(MessagePublisher publisher)
		: base(publisher.socket)
	{
	}

	// I don't like this design, but NetMQ's SocketOptions has a dependency
	// on Socket which seems backwards

	// inhertited interface members
	//   SendHighWatermark
}
