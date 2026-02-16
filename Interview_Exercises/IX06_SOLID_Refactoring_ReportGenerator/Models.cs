using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IX06_SOLID_Refactoring_ReportGenerator;

public record Employee(
    string Name,
    string Department,
    decimal Salary,
    DateTime HireDate);

public record DepartmentStatistics(
    string Department,
    int EmployeeCount,
    decimal AverageSalary
);

public record EmployeesStatistics(
    int TotalEmployees,
    decimal AverageSalary,
    string MinSalaryEmployee,
    decimal MinSalary,
    string MaxSalaryEmployee,
    decimal MaxSalary,
    IReadOnlyCollection<DepartmentStatistics> Departments
);

