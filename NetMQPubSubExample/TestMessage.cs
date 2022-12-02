namespace NetMQPubSubExample;

using System;
using System.Text.Json.Serialization;

internal class TestMessage
{
	public int Age { get; set; }

	public string Name { get; set; }

	public DateTime Now { get; set; }

	public int Counter { get; set; }

	[JsonIgnore]
	public int HiddenProperty { get; set; }

	public TestMessage()
	{
		var names = new string[] { "Joe", "Sally", "Mary", "Steve", "Iris", "Bob" };
		var random = new Random();
		this.Age = random.Next(1, 100);
		this.Name = names[random.Next(names.Length)];
		this.Now = DateTime.Now;
		this.Counter = 0;
		this.HiddenProperty = 666;
	}
}
