namespace NetMQPubSub.Core.Interfaces;

/// <summary>
/// Interface for providing message publisher options.
/// </summary>
public interface IMessagePublisherOptions
{
	/// <summary>
	/// Get or set the high-water-mark for transmission.
	/// This is a hard limit on the number of messages that are allowed to queue up
	/// before mitigative action is taken.
	/// The default is 1000.
	/// </summary>
	int SendHighWatermark { get; set; }
}
