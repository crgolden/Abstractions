namespace crgolden.Abstractions
{
    using System;
    using System.Linq;
    using Hangfire;
    using Hangfire.Server;
    using MediatR;

    public abstract class HangfireJobService : JobService
    {
        protected readonly IBackgroundJobClient BackgroundJobClient;
        protected readonly IRecurringJobManager RecurringJobManager;

        protected HangfireJobService(
            IMediator mediator,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager)
            : base(mediator)
        {
            BackgroundJobClient = backgroundJobClient;
            RecurringJobManager = recurringJobManager;
        }

        protected virtual DateTime? GetCompareDate(PerformContext context, string methodName)
        {
            return long.TryParse(context.BackgroundJob.Id, out var currentJobId)
                ? JobStorage.Current
                    .GetMonitoringApi()
                    .SucceededJobs(0, (int)currentJobId)
                    .LastOrDefault(x => x.Value.Job.Method.Name == methodName).Value?.SucceededAt
                : null;
        }
    }
}
