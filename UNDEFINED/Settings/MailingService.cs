using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace UNDEFINED.Settings
{
	public class MailingService : IMailingService
	{
		private readonly MailSettings _mailSettings;

		public MailingService(IOptions<MailSettings> mailSettings)
		{
			this._mailSettings = mailSettings.Value;
		}

		public async Task SendEmailAsync(string mailTo, string subject, string body)
		{
			var email = new MimeMessage
			{
				Sender = MailboxAddress.Parse(_mailSettings.Email),
				Subject = subject,
			};

			email.To.Add(MailboxAddress.Parse(mailTo));

			var builder = new BodyBuilder();
			builder.HtmlBody = body;
			email.Body = builder.ToMessageBody();
			email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

			using var smtp = new SmtpClient();

			smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
			smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);

			await smtp.SendAsync(email);

			smtp.Disconnect(true);
		}
	}
}
