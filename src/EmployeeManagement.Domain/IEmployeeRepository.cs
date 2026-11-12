namespace EmployeeManagement.Domain;

public interface IEmployeeRepository
{
    IReadOnlyCollection<Employee> GetAll();
    Employee? GetById(int id);
    void Add(Employee employee);
    Task<IReadOnlyCollection<Employee>> GetAllAsync(CancellationToken cancellationToken = default);
}
