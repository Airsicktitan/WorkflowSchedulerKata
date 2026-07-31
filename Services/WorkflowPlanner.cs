using WorkflowSchedulerKata.Domain;

namespace WorkflowSchedulerKata.Services;

public class WorkflowPlanner
{
    public IReadOnlyCollection<WorkflowJob> GetReadyJobs(IReadOnlyCollection<WorkflowJob> jobs, IReadOnlyCollection<int> completedJobIds)
    {
        ArgumentNullException.ThrowIfNull(jobs);
        ArgumentNullException.ThrowIfNull(completedJobIds);

        IReadOnlyCollection<WorkflowJob> incompleteJobs = jobs
            .Where(job => !completedJobIds.Contains(job.Id))
            .ToArray();

        IReadOnlyCollection<WorkflowJob> readyJobs = incompleteJobs
            .Where(job => job.DependencyIds
                .All(dependencyId => completedJobIds.Contains(dependencyId)))
                .OrderByDescending(job => job.Priority switch
                {
                    JobPriority.Critical => 4,
                    JobPriority.High => 3,
                    JobPriority.Medium => 2,
                    JobPriority.Low => 1,
                    _ => 0
                })
                .ThenBy(job => job.EstimatedDurationMinutes)
                .ThenBy(job => job.Id)
            .ToArray();

        return readyJobs;
    }

    public IReadOnlyCollection<WorkflowJob> GetExecutionPlan(IReadOnlyCollection<WorkflowJob> jobs)
    {
        return ExecuteWorkflow(jobs, []);
    }

    public IReadOnlyCollection<WorkflowJob> ExecuteWorkflow(IReadOnlyCollection<WorkflowJob> jobs, IReadOnlyCollection<int> completedJobIds)
    {
        ArgumentNullException.ThrowIfNull(jobs);
        ArgumentNullException.ThrowIfNull(completedJobIds);

        HashSet<int> completedJobIdSet = completedJobIds.ToHashSet();
        List<WorkflowJob> executedJobs = [];

        while (jobs.Any(job => !completedJobIdSet.Contains(job.Id)))
        {
            IReadOnlyCollection<WorkflowJob> readyJobs = GetReadyJobs(jobs, completedJobIdSet);

            if (readyJobs.Count == 0)
            {
                throw new InvalidOperationException("The workflow cannot be completed because no remaining jobs are ready.");
            }

            WorkflowJob nextJob = readyJobs.First();
            executedJobs.Add(nextJob);
            completedJobIdSet.Add(nextJob.Id);
        }

        return executedJobs.AsReadOnly();
    }
}