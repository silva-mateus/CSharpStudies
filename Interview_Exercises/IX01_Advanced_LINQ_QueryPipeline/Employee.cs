namespace IX01_Advanced_LINQ_QueryPipeline;

public record Employee(string Name, string Department, decimal Salary, DateTime HireDate);

public record DepartmentSalary(string Department, decimal AverageSalary);

public record DepartmentSummary(
    string Department,
    int EmployeeCount,
    decimal MinSalary,
    decimal MaxSalary,
    decimal AverageSalary);
