namespace NetMQPubSub.Subscriber;

using NetMQ;
using NetMQPubSub.Core.Interfaces;

public class MessageSubscriberOptions : SocketOptions, IMessageSubscriberOptions
{
	public MessageSubscriberOptions(MessageSubscriber publisher)
		: base(publisher.socket)
	{
	}

	// I don't like this design, but NetMQ's SocketOptions has a dependency
	// on Socket which seems backwards

	// inhertited interface members
	//   ReceiveHighWatermark
}
