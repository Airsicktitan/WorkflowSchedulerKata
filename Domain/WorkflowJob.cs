namespace WorkflowSchedulerKata.Domain;

public class WorkflowJob
{
    public int Id { get; }
    public string Name { get;}
    public int EstimatedDurationMinutes { get; }
    public JobPriority Priority { get; }
    public IReadOnlyCollection<int> DependencyIds { get; }

    public WorkflowJob(
        int id,
        string name,
        int estimatedDurationMinutes,
        JobPriority priority,
        IReadOnlyCollection<int> dependencyIds)
    {

        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be blank.", nameof(name));
        }

        if (estimatedDurationMinutes <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(estimatedDurationMinutes), "EstimatedDurationMinutes must be greater than zero.");
        }

        ArgumentNullException.ThrowIfNull(dependencyIds);

        if (dependencyIds.Any(dependencyId => dependencyId <= 0))
        {
            throw new ArgumentOutOfRangeException(nameof(dependencyIds), "Dependency IDs must be greater than zero.");
        }

        if (dependencyIds.Contains(id))
        {
            throw new ArgumentException("A job cannot depend on itself.", nameof(dependencyIds));
        }

        if (dependencyIds.Count != dependencyIds.Distinct().Count())
        {
            throw new ArgumentException("Dependency IDs cannot contain duplicates.", nameof(dependencyIds));
        }

        Id = id;
        Name = name;
        EstimatedDurationMinutes = estimatedDurationMinutes;
        Priority = priority;
        DependencyIds = [.. dependencyIds];
    }

}