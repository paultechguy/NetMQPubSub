namespace NetMQPubSub.Core.Interfaces;

using System;

public interface IMessageSubscriber : IDisposable
{
	void Connect(string address);

	void Disconnect(string address);

	void Close();

	string ReceiveStringMessage();

	T ReceiveMessage<T>() where T : class, new();

	bool TryReceiveStringMessage(TimeSpan timeout, out string? topic, out string? message);

	bool TryReceiveMessage<T>(TimeSpan timeout, out string? topic, out T? entity) where T : class, new();

	void TopicSubscribe(string topic);

	void TopicSubscribe(IEnumerable<string> topics);

	IMessageSubscriberOptions Options { get; }
}
