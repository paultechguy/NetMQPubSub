namespace NetMQPubSub.Core.Interfaces;

/// <summary>
/// Interface for providing message subscriber options.
/// </summary>
public interface IMessageSubscriberOptions
{
	/// <summary>
	/// Get or set the high-water-mark for reception.
	/// This is a hard limit on the number of messages that are allowed to queue up
	/// before mitigative action is taken.
	/// The default is 1000.
	/// </summary>
	int ReceiveHighWatermark { get; set; }
}
