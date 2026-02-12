using IX01_Advanced_LINQ_QueryPipeline;

var employees = new List<Employee>
{
    new("Alice Johnson", "Engineering", 120_000m, new DateTime(2019, 3, 15)),
    new("Bob Smith", "Engineering", 110_000m, new DateTime(2020, 7, 1)),
    new("Carol White", "Engineering", 95_000m, new DateTime(2023, 1, 10)),
    new("David Brown", "Marketing", 85_000m, new DateTime(2018, 11, 20)),
    new("Eve Davis", "Marketing", 92_000m, new DateTime(2021, 5, 5)),
    new("Frank Miller", "Sales", 78_000m, new DateTime(2022, 8, 14)),
    new("Grace Wilson", "Sales", 105_000m, new DateTime(2017, 2, 28)),
    new("Henry Taylor", "HR", 72_000m, new DateTime(2020, 9, 3)),
    new("Ivy Anderson", "HR", 68_000m, new DateTime(2024, 4, 22)),
    new("Jack Thomas", "Engineering", 130_000m, new DateTime(2016, 6, 18)),
};

var service = new EmployeeQueryService(employees);

Console.WriteLine("=== Average Salary by Department ===");
foreach (var dept in service.GetAverageSalaryByDepartment())
{
    Console.WriteLine($"  {dept.Department}: {dept.AverageSalary:C}");
}

Console.WriteLine("\n=== Top 3 Earners ===");
foreach (var emp in service.GetTopEarners(3))
{
    Console.WriteLine($"  {emp.Name} ({emp.Department}): {emp.Salary:C}");
}

Console.WriteLine("\n=== Recent High Earners (last 5 years, > $90,000) ===");
foreach (var emp in service.GetRecentHighEarners(5, 90_000m))
{
    Console.WriteLine($"  {emp.Name} - Hired: {emp.HireDate:yyyy-MM-dd} - Salary: {emp.Salary:C}");
}

Console.WriteLine("\n=== Department Summaries ===");
foreach (var summary in service.GetDepartmentSummaries())
{
    Console.WriteLine($"  {summary.Department}: Count={summary.EmployeeCount}, " +
                      $"Min={summary.MinSalary:C}, Max={summary.MaxSalary:C}, Avg={summary.AverageSalary:C}");
}

Console.WriteLine("\n=== Departments Above $90,000 Budget ===");
foreach (var dept in service.GetDepartmentsAboveBudget(90_000m))
{
    Console.WriteLine($"  {dept}");
}
