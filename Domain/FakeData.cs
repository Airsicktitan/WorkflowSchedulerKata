using WorkflowSchedulerKata.Domain;

namespace WorkflowSchedulerKata.Data;

public static class FakeData
{
    public static IReadOnlyCollection<WorkflowJob> GetJobs()
    {
        return new List<WorkflowJob>
        {
            new(
                id: 1,
                name: "Download Customer Data",
                estimatedDurationMinutes: 4,
                priority: JobPriority.High,
                dependencyIds: []),

            new(
                id: 2,
                name: "Download Product Data",
                estimatedDurationMinutes: 3,
                priority: JobPriority.Medium,
                dependencyIds: []),

            new(
                id: 3,
                name: "Validate Customer Data",
                estimatedDurationMinutes: 2,
                priority: JobPriority.High,
                dependencyIds: [1]),

            new(
                id: 4,
                name: "Validate Product Data",
                estimatedDurationMinutes: 2,
                priority: JobPriority.Medium,
                dependencyIds: [2]),

            new(
                id: 5,
                name: "Import Customer Data",
                estimatedDurationMinutes: 5,
                priority: JobPriority.Critical,
                dependencyIds: [3]),

            new(
                id: 6,
                name: "Import Product Data",
                estimatedDurationMinutes: 4,
                priority: JobPriority.High,
                dependencyIds: [4]),

            new(
                id: 7,
                name: "Generate Import Summary",
                estimatedDurationMinutes: 2,
                priority: JobPriority.Low,
                dependencyIds: [5, 6]),

            new(
                id: 8,
                name: "Send Completion Notification",
                estimatedDurationMinutes: 1,
                priority: JobPriority.Low,
                dependencyIds: [7])
        }.AsReadOnly();
    }
}