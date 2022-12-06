namespace NetMQPubSub.WebApp.Pages;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NetMQPubSub.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

public class IndexModel : PageModel
{
	private const string MessageTopic = "JobMessage";
	private readonly IMessagePublisher publisher;
	private readonly ILogger<IndexModel> logger;

	[BindProperty]
	[Required]
	[StringLength(99999, MinimumLength = 1)]
	public string Message { get; set; } = string.Empty;

	public IndexModel(
		IMessagePublisher publisher,
		ILogger<IndexModel> logger)
	{
		this.publisher = publisher;
		this.logger = logger;
	}

	public void OnGet()
	{
	}

	public IActionResult OnPostSendMessage()
	{
		this.logger.LogInformation($"==> Sending topic {MessageTopic} message \"{this.Message}\"");

		this.publisher.SendTopicMessage(MessageTopic, this.Message);

		return this.RedirectToPage("/Index");
	}
}
