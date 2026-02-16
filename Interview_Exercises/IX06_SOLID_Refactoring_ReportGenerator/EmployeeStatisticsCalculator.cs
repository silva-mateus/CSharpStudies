using IX06_SOLID_Refactoring_ReportGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IX06_SOLID_Refactoring_ReportGenerator;
public class EmployeeStatisticsCalculator : IStatisticsCalculator
{
    public EmployeesStatistics Calculate(IReadOnlyCollection<Employee> employees)
    {
        if (employees.Count == 0)
        {
            return new EmployeesStatistics(
                0, 0m,  string.Empty, 0m, string.Empty, 0m, Array.Empty<DepartmentStatistics>());
        }

        var totalEmployees = employees.Count;
        var totalSalary = employees.Sum(e => e.Salary);

        var minEmployee = employees.MinBy(e => e.Salary);
        var maxEmployee = employees.MaxBy(e => e.Salary);

        var departments = employees
            .GroupBy(e => e.Department)
            .Select(g => new DepartmentStatistics(
                g.Key,
                g.Count(),
                g.Average(e => e.Salary)))
            .OrderBy(d => d.Department)
            .ToList();

        return new EmployeesStatistics(
            totalEmployees, 
            totalSalary / totalEmployees,
            minEmployee.Name,
            minEmployee.Salary,
            maxEmployee.Name, 
            maxEmployee.Salary, 
            departments);

    }
}

