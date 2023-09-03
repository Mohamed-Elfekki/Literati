namespace Literati.Settings
{
	public interface IMailingService
	{
		Task SendEmailAsync(string mailTo, string subject, string body);
	}
}
