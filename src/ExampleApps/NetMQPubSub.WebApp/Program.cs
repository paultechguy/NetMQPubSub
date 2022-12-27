namespace NetMQPubSub.WebApp;

using NetMQPubSub.Common.Helpers;
using NetMQPubSub.Core.Interfaces;
using NetMQPubSub.WebApp.BackgroundSubscriber;
using NetMQPubSub.WebApp.ExamplePublisher;

public class Program
{
		private const string BindAddress = "inproc://admin-bulk-email"; // or something like "tcp://localhost:54321";

	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddRazorPages();

		// it is critical we bind here, early before the subscriber service
		// is hsoted, so subscribers can subscribe to the publisher
		var publisher = new ExampleJobPublisher();
		publisher.Bind(BindAddress);
		builder.Services.AddSingleton<IMessagePublisher>(publisher);
		builder.Services.AddHostedService<SubscriberJobService>();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthorization();

		app.MapRazorPages();

		app.Run();

		// clean up IPC messaging
		NetMQPubSubHelper.Cleanup(block: true);
	}
}