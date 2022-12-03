namespace NetMQPubSub.Shared.Interfaces;

using System;

public interface IMessageSubscriber : IDisposable
{
	void Connect(string address);

	void Disconnect(string address);

	void Close();

	bool TryReceiveTopicMessage(TimeSpan timeout, out string? topic, out bool moreFrames);

	string? ReceiveMessage();

	T ReceiveMessage<T>() where T : class, new();

	bool TryReceiveTopicMessage<T>(TimeSpan timeout, out string? topic, out T? entity) where T : class, new();

	void TopicSubscribe(string topic);

	void TopicSubscribe(IEnumerable<string> topics);

	IMessageSubscriberOptions Options { get; }
}
