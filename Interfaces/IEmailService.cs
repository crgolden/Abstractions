namespace Clarity.Abstractions
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task SendEmailAsync(
            string email,
            string subject,
            string htmlMessage,
            CancellationToken token);

        Task SendEmailAsync(
            IDictionary<string, object> userProperties,
            byte[] body,
            CancellationToken token);
    }
}
