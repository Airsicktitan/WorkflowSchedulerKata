using WorkflowSchedulerKata.Data;
using WorkflowSchedulerKata.Domain;
using WorkflowSchedulerKata.Services;
namespace WorkflowSchedulerKata;

class Program
{
    static void Main(string[] args)
    {
        IReadOnlyCollection<WorkflowJob> jobs = FakeData.GetJobs();
        IReadOnlyCollection<int> completedJobIds = [1, 2];

        var workflowPlanner = new WorkflowPlanner();

        IReadOnlyCollection<WorkflowJob> readyJobs = workflowPlanner.GetReadyJobs(jobs, completedJobIds);

        Console.WriteLine($"Completed Jobs: {string.Join(", ", completedJobIds)}");

        Console.WriteLine("\nReady jobs:");

        foreach (var job in readyJobs)
        {
            Console.WriteLine($"Job {job.Id}: {job.Name}");
        }
        
    }
}
