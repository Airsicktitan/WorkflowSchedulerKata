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
            .Where(job => job.DependencyIds.All(dependencyId => completedJobIds.Contains(dependencyId)))
            .ToArray();

        return readyJobs;
    }
}