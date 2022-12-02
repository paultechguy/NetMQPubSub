namespace NetMQPubSubExample;

using NetMQ;
using NetMQ.Sockets;
using System;
using System.Text.Json;

public class MessagePublisher : BaseMessagePublisher, IMessagePublisher, IDisposable
{
	internal readonly PublisherSocket socket;
	private bool disposedValue;

	public MessagePublisherOptions Options { get; }

	public MessagePublisher()
		: base()
	{
		this.socket = new PublisherSocket();
		this.Options = new MessagePublisherOptions(this);
	}

	public void Bind(string address)
	{
		this.socket.Bind(address);
	}

	public void SendTopicMessage(string topic, string message)
	{
		this.socket.SendMoreFrame(topic).SendFrame(message);
	}

	public void SendTopicMessage<T>(string topic, T entity) where T : class, new()
	{
		var json = JsonSerializer.Serialize(entity);
		this.socket.SendMoreFrame(topic).SendFrame(json);
	}

	public void Unbind(string address)
	{
		this.socket.Unbind(address);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				// TODO: dispose managed state (managed objects)
			}

			// TODO: free unmanaged resources (unmanaged objects) and override finalizer
			// TODO: set large fields to null
			this.socket.Dispose();
			disposedValue = true;
		}
	}

	// override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
	~MessagePublisher()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
