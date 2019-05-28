namespace Clarity.Abstractions
{
    using Hangfire;
    using Hangfire.Common;
    using Hangfire.Server;
    using Hangfire.Storage;
    using MediatR;
    using System;
    using System.Linq;

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
            if (!long.TryParse(context.BackgroundJob.Id, out var currentJobId)) return null;
            using (var connection = context.Storage.GetConnection())
            {
                var recurringJobId = context.GetJobParameter<string>("RecurringJobId");
                if (string.IsNullOrEmpty(recurringJobId)) return null;
                var recurringJob = connection.GetRecurringJobs().Single(p => p.Id == recurringJobId);
                JobData lastJobData = null;
                StateData lastStateData = null;
                do
                {
                    var lastJob = connection.GetJobData($"{--currentJobId}");
                    if (lastJob?.Job?.Method?.Name == methodName && lastJob.State == "Succeeded")
                    {
                        lastJobData = lastJob;
                    }
                } while (lastJobData == null && currentJobId > 0);

                if (lastJobData != null) lastStateData = connection.GetStateData($"{currentJobId}");
                if (lastStateData?.Data.ContainsKey("SucceededAt") == true)
                {
                    var succeededAt = lastStateData.Data["SucceededAt"];
                    return JobHelper.DeserializeNullableDateTime(succeededAt)?.ToLocalTime();
                }

                return null;
            }
        }
    }
}
