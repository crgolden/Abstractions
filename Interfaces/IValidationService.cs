namespace Clarity.Abstractions
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IValidationService<in TModel>
    {
        Task<bool> ValidateAsync(TModel model, CancellationToken token);
    }
}
