namespace IX01_Advanced_LINQ_QueryPipeline;

public class EmployeeQueryService
{
    private readonly List<Employee> _employees;

    public EmployeeQueryService(List<Employee> employees)
    {
        _employees = employees;
    }

    /// <summary>
    /// Returns average salary per department, ordered by department name ascending.
    /// </summary>
    public IEnumerable<DepartmentSalary> GetAverageSalaryByDepartment()
    {
        return _employees
            .GroupBy(e => e.Department)
            .Select(group => new DepartmentSalary(group.Key, group.Average(e => e.Salary)))
            .OrderBy(ds => ds.Department);
    }

    /// <summary>
    /// Returns the top N earners by salary descending, then by name ascending for ties.
    /// </summary>
    public IEnumerable<Employee> GetTopEarners(int count)
    {
        return _employees
            .OrderByDescending(e => e.Salary)
            .ThenBy(e => e.Name)
            .Take(count);
    }

    /// <summary>
    /// Returns employees hired within the last <paramref name="yearsBack"/> years
    /// whose salary exceeds <paramref name="salaryThreshold"/>.
    /// Ordered by hire date descending.
    /// </summary>
    public IEnumerable<Employee> GetRecentHighEarners(int yearsBack, decimal salaryThreshold)
    {
        return _employees
            .Where(e => e.HireDate >= DateTime.Now.AddYears(-yearsBack))
            .Where(e => e.Salary >  salaryThreshold)
            .OrderByDescending(e => e.HireDate);
    }

    /// <summary>
    /// Returns a summary per department with employee count, min/max/avg salary.
    /// Ordered by employee count descending, then department name ascending.
    /// </summary>
    public IEnumerable<DepartmentSummary> GetDepartmentSummaries()
    {
        return _employees
            .GroupBy(e => e.Department)
            .Select(group => new DepartmentSummary(
                group.Key,
                group.Count(),
                group.Min(e => e.Salary),
                group.Max(e => e.Salary),
                group.Average(e => e.Salary)))
            .OrderByDescending(ds => ds.EmployeeCount)
            .ThenBy(ds => ds.Department);
    }

    /// <summary>
    /// Returns department names where the average salary exceeds the given budget per employee.
    /// Ordered alphabetically.
    /// </summary>
    public IEnumerable<string> GetDepartmentsAboveBudget(decimal budgetPerEmployee)
    {
        // TODO: your code goes here
        return _employees
            .GroupBy(e => e.Department)
            .Where(group => group.Average(e => e.Salary) > budgetPerEmployee)
            .Select(group => group.Key)
            .OrderBy(d => d);
    }
}
