namespace NetMQPubSub.Shared.Helpers;

using NetMQ;

public static class NetMQHelper
{
	public static void Cleanup()
	{
		NetMQConfig.Cleanup(); // the the docs for final cleanup
	}
}
