using System.Globalization;
using EmployeeManagement.Domain;

var departments = new[]
{
    new Department(1, "Engineering"),
    new Department(2, "People")
};

var repository = new InMemoryEmployeeRepository(new[]
{
    new Employee(1, "Asha Patel", "asha@example.com", 1, 95000m),
    new Employee(2, "Noah Kim", "noah@example.com", 1, 88000m),
    new Employee(3, "Mia Chen", "mia@example.com", 2, 72000m)
});
var service = new EmployeeService(repository, departments);
var isRunning = true;

Console.WriteLine("Employee Management");

while (isRunning)
{
    Console.WriteLine();
    Console.WriteLine("Choose an option:");
    Console.WriteLine("1. Read all employees");
    Console.WriteLine("2. Add an employee");
    Console.WriteLine("3. Search employees by name");
    Console.WriteLine("4. Load employees asynchronously");
    Console.WriteLine("0. Exit");
    Console.Write("Option: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1":
            DisplayEmployees(service.GetSummaries());
            break;
        case "2":
            AddEmployee(repository, departments);
            break;
        case "3":
            SearchEmployees(service, departments);
            break;
        case "4":
        {
            var employees = await service.GetAllAsync();
            Console.WriteLine($"Loaded {employees.Count} employees asynchronously.");
            break;
        }
        case "0":
            isRunning = false;
            Console.WriteLine("Goodbye.");
            break;
        default:
            Console.WriteLine("Please enter 1, 2, 3, 4, or 0.");
            break;
    }
}

static void DisplayEmployees(IEnumerable<EmployeeSummary> employees)
{
    var employeeList = employees.OrderBy(employee => employee.Name).ToArray();
    if (employeeList.Length == 0)
    {
        Console.WriteLine("No employees found.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine($"{"ID",-5} {"Name",-20} {"Department",-15} {"Email",-28} Salary");
    Console.WriteLine(new string('-', 85));
    foreach (var employee in employeeList)
        Console.WriteLine($"{employee.Id,-5} {employee.Name,-20} {employee.Department,-15} {employee.Email,-28} ${employee.Salary:N2}");
}

static void AddEmployee(IEmployeeRepository repository, IReadOnlyCollection<Department> departments)
{
    Console.WriteLine();
    Console.WriteLine("Add employee");

    var name = ReadRequiredText("Name: ");
    if (name is null) return;

    var email = ReadRequiredText("Email: ");
    if (email is null) return;

    Console.WriteLine("Departments:");
    foreach (var department in departments)
        Console.WriteLine($"{department.Id}. {department.Name}");

    var departmentId = ReadDepartmentId(departments);
    if (departmentId is null) return;

    var salary = ReadSalary();
    if (salary is null) return;

    var nextId = repository.GetAll().Select(employee => employee.Id).DefaultIfEmpty(0).Max() + 1;
    try
    {
        var employee = new Employee(nextId, name, email, departmentId.Value, salary.Value);
        repository.Add(employee);
        Console.WriteLine($"Added {employee.Name} with employee ID {employee.Id}.");
        Console.WriteLine("Choose '1. Read all employees' to display the updated list.");
    }
    catch (ArgumentException exception)
    {
        Console.WriteLine($"Employee was not added: {exception.Message}");
    }
    catch (InvalidOperationException exception)
    {
        Console.WriteLine($"Employee was not added: {exception.Message}");
    }
}

static string? ReadRequiredText(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var value = Console.ReadLine();
        if (value is null) return null;
        if (!string.IsNullOrWhiteSpace(value)) return value.Trim();
        Console.WriteLine("A value is required.");
    }
}

static int? ReadDepartmentId(IReadOnlyCollection<Department> departments)
{
    while (true)
    {
        Console.Write("Department ID: ");
        var input = Console.ReadLine();
        if (input is null) return null;
        if (int.TryParse(input, out var departmentId) && departments.Any(department => department.Id == departmentId))
            return departmentId;
        Console.WriteLine("Enter an ID from the department list.");
    }
}

static decimal? ReadSalary()
{
    while (true)
    {
        Console.Write("Salary (for example, 75000): $");
        var input = Console.ReadLine();
        if (input is null) return null;
        if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var salary) && salary >= 0)
            return salary;
        Console.WriteLine("Enter a valid non-negative salary.");
    }
}

static void SearchEmployees(EmployeeService service, IReadOnlyCollection<Department> departments)
{
    Console.Write("Name to search: ");
    var term = Console.ReadLine();
    if (term is null) return;
    var matches = service.FindByName(term.Trim())
        .Select(employee => new EmployeeSummary(
            employee.Id,
            employee.Name,
            departments.FirstOrDefault(department => department.Id == employee.DepartmentId)?.Name ?? "Unknown",
            employee.Email,
            employee.Salary));
    DisplayEmployees(matches);
}
