using WorkflowSchedulerKata.Data;
using WorkflowSchedulerKata.Domain;
using WorkflowSchedulerKata.Services;

namespace WorkflowSchedulerKata.Tests;

public class WorkflowPlannerTests
{
    [Fact]
    public void GetReadyJobs_WhenNoJobsAreCompleted_ReturnsInitialReadyJobsInOrder()
    {
        // Arrange
        IReadOnlyCollection<WorkflowJob> jobs = FakeData.GetJobs();
        IReadOnlyCollection<int> completedJobIds = [];

        var planner = new WorkflowPlanner();

        int[] expectedJobIds = [1, 2];

        // Act
        IReadOnlyCollection<WorkflowJob> readyJobs =
            planner.GetReadyJobs(jobs, completedJobIds);

        int[] actualJobIds = readyJobs
            .Select(job => job.Id)
            .ToArray();

        // Assert
        Assert.Equal(expectedJobIds, actualJobIds);
    }

    [Fact]
    public void GetExecutionPlan_WhenNoJobsAreInitiallyCompleted_ReturnsExpectedOrder()
    {
        // Arrange
        IReadOnlyCollection<WorkflowJob> jobs = FakeData.GetJobs();

        var planner = new WorkflowPlanner();

        int[] expectedJobIds = [1, 3, 5, 2, 4, 6, 7, 8];

        // Act
        IReadOnlyCollection<WorkflowJob> executionPlan =
            planner.GetExecutionPlan(jobs);

        int[] actualJobIds = executionPlan
            .Select(job => job.Id)
            .ToArray();

        // Assert
        Assert.Equal(expectedJobIds, actualJobIds);
    }

    [Fact]
    public void ExecuteWorkflow_WhenSomeJobsAreAlreadyCompleted_SkipsCompletedJobs()
    {
        // Arrange
        IReadOnlyCollection<WorkflowJob> jobs = FakeData.GetJobs();
        IReadOnlyCollection<int> completedJobIds = [1, 2];

        var planner = new WorkflowPlanner();

        int[] expectedJobIds = [3, 5, 4, 6, 7, 8];

        // Act
        IReadOnlyCollection<WorkflowJob> executedJobs =
            planner.ExecuteWorkflow(jobs, completedJobIds);

        int[] actualJobIds = executedJobs
            .Select(job => job.Id)
            .ToArray();

        // Assert
        Assert.Equal(expectedJobIds, actualJobIds);

        Assert.DoesNotContain(
            executedJobs,
            job => completedJobIds.Contains(job.Id));
    }

    [Fact]
    public void ExecuteWorkflow_WhenAllJobsAreAlreadyCompleted_ReturnsEmptyCollection()
    {
        // Arrange
        IReadOnlyCollection<WorkflowJob> jobs = FakeData.GetJobs();

        IReadOnlyCollection<int> completedJobIds = jobs
            .Select(job => job.Id)
            .ToArray();

        var planner = new WorkflowPlanner();

        // Act
        IReadOnlyCollection<WorkflowJob> executedJobs =
            planner.ExecuteWorkflow(jobs, completedJobIds);

        // Assert
        Assert.Empty(executedJobs);
    }

    [Fact]
    public void ExecuteWorkflow_DoesNotModifyOriginalCompletedJobIds()
    {
        // Arrange
        IReadOnlyCollection<WorkflowJob> jobs = FakeData.GetJobs();
        int[] completedJobIds = [1, 2];

        var planner = new WorkflowPlanner();

        int[] expectedCompletedJobIds = [1, 2];

        // Act
        planner.ExecuteWorkflow(jobs, completedJobIds);

        // Assert
        Assert.Equal(expectedCompletedJobIds, completedJobIds);
    }

    [Fact]
    public void ExecuteWorkflow_WhenJobsHaveCircularDependencies_ThrowsInvalidOperationException()
    {
        // Arrange
        IReadOnlyCollection<WorkflowJob> jobs =
        [
            new WorkflowJob(
                id: 1,
                name: "First Circular Job",
                estimatedDurationMinutes: 5,
                priority: JobPriority.High,
                dependencyIds: [2]),

            new WorkflowJob(
                id: 2,
                name: "Second Circular Job",
                estimatedDurationMinutes: 5,
                priority: JobPriority.High,
                dependencyIds: [1])
        ];

        var planner = new WorkflowPlanner();

        // Act
        Action executeWorkflow = () =>
            planner.ExecuteWorkflow(jobs, []);

        // Assert
        InvalidOperationException exception =
            Assert.Throws<InvalidOperationException>(executeWorkflow);

        Assert.Equal(
            "The workflow cannot be completed because no remaining jobs are ready.",
            exception.Message);
    }

    [Fact]
    public void ExecuteWorkflow_WhenJobHasMissingDependency_ThrowsInvalidOperationException()
    {
        // Arrange
        IReadOnlyCollection<WorkflowJob> jobs =
        [
            new WorkflowJob(
                id: 1,
                name: "Blocked Job",
                estimatedDurationMinutes: 5,
                priority: JobPriority.High,
                dependencyIds: [99])
        ];

        var planner = new WorkflowPlanner();

        // Act
        Action executeWorkflow = () =>
            planner.ExecuteWorkflow(jobs, []);

        // Assert
        InvalidOperationException exception =
            Assert.Throws<InvalidOperationException>(executeWorkflow);

        Assert.Equal(
            "The workflow cannot be completed because no remaining jobs are ready.",
            exception.Message);
    }

    [Fact]
    public void ExecuteWorkflow_WhenJobsIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var planner = new WorkflowPlanner();

        // Act
        Action executeWorkflow = () =>
            planner.ExecuteWorkflow(null!, []);

        // Assert
        Assert.Throws<ArgumentNullException>(executeWorkflow);
    }

    [Fact]
    public void ExecuteWorkflow_WhenCompletedJobIdsIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IReadOnlyCollection<WorkflowJob> jobs = FakeData.GetJobs();

        var planner = new WorkflowPlanner();

        // Act
        Action executeWorkflow = () =>
            planner.ExecuteWorkflow(jobs, null!);

        // Assert
        Assert.Throws<ArgumentNullException>(executeWorkflow);
    }
}