namespace NetMQPubSub.WebApp;

using NetMQPubSub.Common.Helpers;
using NetMQPubSub.Core.Interfaces;
using NetMQPubSub.Subscriber;
using NetMQPubSub.WebApp.BackgroundSubscriber;
using NetMQPubSub.WebApp.ExamplePublisher;

public class Program
{
	private const string BindAddress = "tcp://localhost:54321";

	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddRazorPages();
		builder.Services.AddSingleton<IMessagePublisher>(s =>
		{
			// it is critical we bind here, early, so subscribers can
			// successful subscribe to this publisher
			var publisher = new ExampleJobPublisher();
			publisher.Bind(BindAddress);
			return publisher;
		});
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
		NetMQPubSubHelper.Cleanup();
	}
}