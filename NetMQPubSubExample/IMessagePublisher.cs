namespace NetMQPubSubExample;

using System;

public interface IMessagePublisher : IDisposable
{
	void Bind(string address);

	void Unbind(string address);

	void SendTopicMessage(string topic, string message);

	void SendTopicMessage<T>(string topic, T entity) where T : class, new();

	MessagePublisherOptions Options { get; }
}
