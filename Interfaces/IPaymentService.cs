namespace Clarity.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IPaymentService
    {
        Task<string> GetCustomerAsync(
            string customerId,
            CancellationToken token);

        Task<string> CreateCustomerAsync(
            string email,
            string tokenId,
            CancellationToken token);

        Task<string> AuthorizeAsync(
            string customerId,
            decimal amount,
            string currency,
            CancellationToken token,
            string description = null);

        Task<string> CaptureAsync(
            string customerId,
            decimal amount,
            string currency,
            CancellationToken token,
            string description = null);

        Task UpdateAsync(
            string chargeId,
            string description,
            CancellationToken token);
    }
}