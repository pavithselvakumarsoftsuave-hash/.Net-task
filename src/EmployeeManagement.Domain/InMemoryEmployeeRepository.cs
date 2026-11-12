namespace EmployeeManagement.Domain;

public sealed class InMemoryEmployeeRepository : IEmployeeRepository
{
    private readonly List<Employee> _employees;
    public InMemoryEmployeeRepository(IEnumerable<Employee>? seed = null) => _employees = seed?.ToList() ?? [];
    public IReadOnlyCollection<Employee> GetAll() => _employees.AsReadOnly();
    public Employee? GetById(int id) => _employees.FirstOrDefault(employee => employee.Id == id);
    public void Add(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);
        if (_employees.Any(existing => existing.Id == employee.Id)) throw new InvalidOperationException($"Employee id {employee.Id} already exists.");
        _employees.Add(employee);
    }
    public async Task<IReadOnlyCollection<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await Task.Yield();
        cancellationToken.ThrowIfCancellationRequested();
        return GetAll();
    }
}
