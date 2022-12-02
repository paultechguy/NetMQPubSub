namespace NetMQPubSub.Subscriber;

using NetMQ;
using NetMQ.Sockets;
using System;
using System.Text.Json;

public class MessageSubscriber : BaseMessageSubscriber, IMessageSubscriber, IDisposable
{
	internal readonly SubscriberSocket socket;
	private bool disposedValue;

	public MessageSubscriberOptions Options { get; }

	public MessageSubscriber()
		: base()
	{
		this.socket = new SubscriberSocket();
		this.Options = new MessageSubscriberOptions(this);
	}

	public virtual void Connect(string address)
	{
		this.socket.Connect(address);
	}

	public virtual void SendTopicMessage(string topic, string message)
	{
		this.socket.SendMoreFrame(topic).SendFrame(message);
	}

	public virtual void Disconnect(string address)
	{
		this.socket.Disconnect(address);
	}

	public virtual void Close()
	{
		this.socket.Close();
	}

	public bool TryReceiveTopicMessage(TimeSpan timeout, out string? topic, out bool moreFrames)
	{
		return this.socket.TryReceiveFrameString(timeout, out topic, out moreFrames);
	}

	public string? ReceiveMessage()
	{
		return this.socket.ReceiveFrameString();
	}

	public T ReceiveMessage<T>() where T : class, new()
	{
		var text = this.ReceiveMessage();
		if (text is null)
		{
			throw new ArgumentNullException("Empty message received");
		}

		return JsonSerializer.Deserialize<T>(text) ?? throw new ArgumentNullException($"Message received not type {typeof(T)}");
	}

	public void TopicSubscribe(string topic)
	{
		this.socket.Subscribe(topic);
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
	~MessageSubscriber()
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
