using WorkflowSchedulerKata.Data;
using WorkflowSchedulerKata.Domain;
using WorkflowSchedulerKata.Services;
namespace WorkflowSchedulerKata;

class Program
{
    static void Main(string[] args)
    {
        IReadOnlyCollection<WorkflowJob> jobs = FakeData.GetJobs();
        IReadOnlyCollection<int> completedJobIds = [];

        var workflowPlanner = new WorkflowPlanner();

        IReadOnlyCollection<WorkflowJob> readyJobs = workflowPlanner.GetReadyJobs(jobs, completedJobIds);
        IReadOnlyCollection<WorkflowJob> executionPlan =  workflowPlanner.GetExecutionPlan(jobs);

        Console.WriteLine($"Completed Jobs: {string.Join(", ", completedJobIds)}");

        Console.WriteLine("\nReady jobs:");

        foreach (var job in readyJobs)
        {
            Console.WriteLine($"Job {job.Id}: {job.Name} - Priority: {job.Priority} - Estimated Duration: {job.EstimatedDurationMinutes}");
        }
        
        Console.WriteLine("\nExecution Plan:");

        foreach (var job in executionPlan)
        {
            Console.WriteLine($"Job {job.Id}: {job.Name} - Priority: {job.Priority} - Estimated Duration: {job.EstimatedDurationMinutes}");
        }
    }
}
