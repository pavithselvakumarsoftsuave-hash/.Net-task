namespace EmployeeManagement.Domain;

public sealed class EmployeeService(IEmployeeRepository employees, IReadOnlyCollection<Department> departments)
{
    public IReadOnlyCollection<Employee> GetAll() => employees.GetAll();
    public IReadOnlyCollection<Employee> FindByName(string term) => employees.GetAll().Where(e => e.Name.Contains(term, StringComparison.OrdinalIgnoreCase)).ToArray();
    public IReadOnlyCollection<Employee> GetByDepartment(int departmentId) => employees.GetAll().Where(e => e.DepartmentId == departmentId).ToArray();
    public IReadOnlyCollection<Employee> GetSortedByName() => employees.GetAll().OrderBy(e => e.Name).ToArray();
    public IReadOnlyCollection<Employee> GetHighestPaid(int count) => employees.GetAll().OrderByDescending(e => e.Salary).Take(Math.Max(0, count)).ToArray();
    public IReadOnlyDictionary<int, int> CountByDepartment() => employees.GetAll().GroupBy(e => e.DepartmentId).ToDictionary(g => g.Key, g => g.Count());
    public IReadOnlyCollection<EmployeeSummary> GetSummaries() => employees.GetAll().Select(e => new EmployeeSummary(e.Id, e.Name, departments.FirstOrDefault(d => d.Id == e.DepartmentId)?.Name ?? "Unknown", e.Email, e.Salary)).ToArray();
    public decimal GetAverageSalary(int departmentId)
    {
        var matching = employees.GetAll().Where(e => e.DepartmentId == departmentId).ToArray();
        return matching.Length == 0 ? 0m : matching.Average(e => e.Salary);
    }
    public Employee? FindByEmail(string email) => employees.GetAll().FirstOrDefault(e => string.Equals(e.Email, email, StringComparison.OrdinalIgnoreCase));
    public Task<IReadOnlyCollection<Employee>> GetAllAsync(CancellationToken cancellationToken = default) => employees.GetAllAsync(cancellationToken);
}

public sealed record EmployeeSummary(int Id, string Name, string Department, string Email, decimal Salary);
