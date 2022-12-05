namespace NetMQPubSub.Subscriber;

using NetMQ;
using NetMQ.Sockets;
using NetMQPubSub.Core.Interfaces;
using System;
using System.Text.Json;

public class MessageSubscriber : BaseMessageSubscriber, IMessageSubscriber, IDisposable
{
	internal readonly SubscriberSocket socket;
	private bool disposedValue;

	public IMessageSubscriberOptions Options { get; }

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

	public string? ReceiveStringMessage()
	{
		return this.socket.ReceiveFrameString();
	}

	public bool TryReceiveStringMessage(TimeSpan timeout, out string? message, out bool moreFrames)
	{
		return this.socket.TryReceiveFrameString(timeout, out message, out moreFrames);
	}

	public T ReceiveMessage<T>() where T : class, new()
	{
		var text = this.ReceiveStringMessage();
		if (text is null)
		{
			throw new ArgumentNullException("Empty message received");
		}

		T entity = JsonSerializer.Deserialize<T>(text) ?? throw new ArgumentNullException($"Message received not type {typeof(T)}");

		return entity;
	}

	public bool TryReceiveStringMessage(TimeSpan timeout, out string? topic, out string? message)
	{
		message = null;
		if (this.TryReceiveStringMessage(timeout, out topic, out bool moreFrames))
		{
			if (moreFrames)
			{
				message = this.ReceiveStringMessage();
			}
		}

		return message is not null;
	}

	public bool TryReceiveMessage<T>(TimeSpan timeout, out string? topic, out T? entity) where T : class, new()
	{
		entity = null;
		if (this.TryReceiveStringMessage(timeout, out topic, out bool moreFrames))
		{
			if (moreFrames)
			{
				entity = this.ReceiveMessage<T>();
			}
		}

		return entity is not null;
	}

	public void TopicSubscribe(string topic)
	{
		this.socket.Subscribe(topic);
	}

	public void TopicSubscribe(IEnumerable<string> topics)
	{
		foreach (var topic in topics)
		{
			this.socket.Subscribe(topic);
		}
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
