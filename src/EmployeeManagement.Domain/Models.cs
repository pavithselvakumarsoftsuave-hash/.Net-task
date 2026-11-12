namespace EmployeeManagement.Domain;

public sealed record Department(int Id, string Name);

public sealed class Employee
{
    public int Id { get; }
    public string Name { get; }
    public string Email { get; }
    public int DepartmentId { get; }
    public decimal Salary { get; }

    public Employee(int id, string name, string email, int departmentId, decimal salary)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be positive.");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@", StringComparison.Ordinal)) throw new ArgumentException("A valid email is required.", nameof(email));
        if (departmentId <= 0) throw new ArgumentOutOfRangeException(nameof(departmentId), "Department id must be positive.");
        if (salary < 0) throw new ArgumentOutOfRangeException(nameof(salary), "Salary cannot be negative.");
        Id = id;
        Name = name.Trim();
        Email = email.Trim();
        DepartmentId = departmentId;
        Salary = salary;
    }
}

