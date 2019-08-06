namespace crgolden.Abstractions
{
    using AutoMapper;
    using MediatR;
    using Shared;

    public abstract class JobService : IJobService
    {
        protected readonly IMediator Mediator;
        protected readonly IMapper Mapper;

        protected JobService(IMediator mediator, IMapper mapper)
        {
            Mediator = mediator;
            Mapper = mapper;
        }

        public abstract void SetJobs();
    }
}
