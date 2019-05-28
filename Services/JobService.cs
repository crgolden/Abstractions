namespace Clarity.Abstractions
{
    using MediatR;
    using Shared;

    public abstract class JobService : IJobService
    {
        protected readonly IMediator Mediator;

        protected JobService(IMediator mediator)
        {
            Mediator = mediator;
        }

        public abstract void SetJobs();
    }
}
