# Employee Management (Week 1)

A small .NET console application for an in-memory employee and department directory. It demonstrates C# domain modeling, interface-based repository design, LINQ, async cancellation, and unit testing.

## Requirements

.NET 8 SDK. The document does not prescribe an SDK version; this solution targets .NET 8 with nullable reference types enabled.

## Run

From this directory:

```sh
dotnet restore EmployeeManagement.sln
dotnet build EmployeeManagement.sln
dotnet run --project src/EmployeeManagement.App
dotnet test EmployeeManagement.sln
```

## Sample output

```text
Employee Management

Choose an option:
1. Read all employees
2. Add an employee
3. Search employees by name
4. Load employees asynchronously
0. Exit
Option: 2

Add employee
Name: Ravi Kumar
Email: ravi@example.com
Departments:
1. Engineering
2. People
Department ID: 1
Salary (for example, 75000): $80000
Added Ravi Kumar with employee ID 4.
Choose '1. Read all employees' to display the updated list.
```

Employee records are held in memory for the lifetime of the running process. Select option 1 after adding an employee to display the current list; the seeded sample data is recreated when the app restarts.
## Design notes

- `Employee` validates its invariants; `Department` is an immutable record.
- `EmployeeService` depends on `IEmployeeRepository`, making its data source replaceable and the core logic easy to test (dependency inversion).
- `InMemoryEmployeeRepository` owns a `List<Employee>` and rejects duplicate IDs.
- The app uses `async`/`await` and passes a `CancellationToken` through to the repository. A real I/O-backed implementation would use cancellable I/O; `Task.Yield` here only demonstrates asynchronous flow.
- Value types: `int` IDs and `decimal` salary. Reference types: `Employee`, `Department`, `string` and collection interfaces. `string` is immutable; repeated concatenation in loops can allocate, so `StringBuilder` is appropriate for that case.

## LINQ query examples and complexity

Let *n* be the number of employees. Hash-based grouping and dictionary lookup have average-case constant-time operations.

| Query | Example | Output | Time / extra space |
|---|---|---|---|
| Filter | `all.Where(e => e.Salary >= 90000m)` | Asha | O(n) / O(k) |
| Project | `all.Select(e => e.Name)` | Names | O(n) / O(n) |
| Group | `all.GroupBy(e => e.DepartmentId)` | Engineering: 2, People: 1 | O(n) / O(n) |
| Sort | `all.OrderBy(e => e.Name)` | Asha, Mia, Noah | O(n log n) / O(n) |
| Search | `all.FirstOrDefault(e => e.Email == email)` | Matching employee or null | O(n) / O(1) |
| Top N | `all.OrderByDescending(e => e.Salary).Take(2)` | Asha, Noah | O(n log n) / O(n) |
| Aggregate | `all.Average(e => e.Salary)` | Average salary | O(n) / O(1) |
| Join-like projection | `all.Select(e => new { e.Name, Department = departments.FirstOrDefault(d => d.Id == e.DepartmentId)?.Name })` | Employee with department name | O(n·d) / O(n) |

`Where` and `Select` are deferred until enumeration; materializing with `ToArray()` executes the query immediately.

## Tests

The xUnit project covers name filtering, grouping, ordering, empty-group boundaries, invalid salary, duplicate IDs, and cancellation. Run with `dotnet test EmployeeManagement.sln`.

## Class relationships

```text
EmployeeService --uses--> IEmployeeRepository <--implements-- InMemoryEmployeeRepository
      ¦
      +--uses--> Department
      +--returns--> Employee / EmployeeSummary
```

## Week 1 LeetCode practice

Each solution is in `WeekOneChallenges` in the domain library. The approach is to identify the needed collection or invariant and cover empty, minimal, and repeated or ordered inputs.

| Day / problem | Approach | Time / space | Edge cases |
|---|---|---|---|
| Day 1: Concatenation of Array | Allocate a 2n result and copy each item into both halves | O(n) / O(n) | Empty and single-item arrays |
| Day 2: Number of Steps to Reduce a Number to Zero | Divide even values by two; subtract one from odd values | O(log n) / O(1) | Zero and negative input |
| Day 3: Contains Duplicate | Add values to a HashSet and stop at the first duplicate | O(n) expected / O(n) | Empty list and repeated values |
| Day 4: Best Time to Buy and Sell Stock | Track the lowest earlier price and best profit in one pass | O(n) / O(1) | Decreasing prices and one price |
| Day 5: Valid Anagram | Count characters in the first string and consume counts with the second | O(n) / O(k) | Empty, unequal length, and mismatched characters |

The focused unit tests exercise these cases. Lesson: maintaining a small running state often avoids nested loops and keeps both the reasoning and implementation clear.


