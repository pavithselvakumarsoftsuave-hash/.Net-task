using EmployeeManagement.Domain;
using Xunit;

namespace EmployeeManagement.Tests;

public sealed class EmployeeServiceTests
{
    private static (InMemoryEmployeeRepository Repository, EmployeeService Service) CreateService()
    {
        var departments = new[] { new Department(1, "Engineering"), new Department(2, "People") };
        var repository = new InMemoryEmployeeRepository(new[]
        {
            new Employee(1, "Asha Patel", "asha@example.com", 1, 95000m),
            new Employee(2, "Noah Kim", "noah@example.com", 1, 88000m),
            new Employee(3, "Mia Chen", "mia@example.com", 2, 72000m)
        });
        return (repository, new EmployeeService(repository, departments));
    }

    [Fact]
    public void FindByEmail_IsCaseInsensitive_RegressionTest()
    {
        var (_, service) = CreateService();
        Assert.Equal("Asha Patel", service.FindByEmail("ASHA@EXAMPLE.COM")?.Name);
    }

    [Fact]
    public void FindByName_IsCaseInsensitive()
    {
        var (_, service) = CreateService();
        Assert.Single(service.FindByName("ASHA"));
    }

    [Fact]
    public void CountByDepartment_GroupsEmployees()
    {
        var (_, service) = CreateService();
        var counts = service.CountByDepartment();
        Assert.Equal(2, counts[1]);
        Assert.Equal(1, counts[2]);
    }

    [Fact]
    public void GetHighestPaid_SortsDescendingAndLimitsResults()
    {
        var (_, service) = CreateService();
        Assert.Equal("Asha Patel", Assert.Single(service.GetHighestPaid(1)).Name);
    }

    [Fact]
    public void GetAverageSalary_ReturnsZeroForEmptyDepartment()
    {
        var (_, service) = CreateService();
        Assert.Equal(0m, service.GetAverageSalary(99));
    }

    [Fact]
    public void Employee_RejectsNegativeSalary()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Employee(4, "Lee Park", "lee@example.com", 1, -1m));
    }

    [Fact]
    public void Add_RejectsDuplicateId()
    {
        var (repository, _) = CreateService();
        Assert.Throws<InvalidOperationException>(() => repository.Add(new Employee(1, "Duplicate", "duplicate@example.com", 1, 1m)));
    }

    [Fact]
    public async Task GetAllAsync_ObservesCancellation()
    {
        var (repository, _) = CreateService();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => repository.GetAllAsync(cancellation.Token));
    }
}





