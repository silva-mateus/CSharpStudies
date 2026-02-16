using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IX06_SOLID_Refactoring_ReportGenerator;
public class PlainTextReportFormatter : IReportFormatter
{
    public string Format(EmployeesStatistics stats)
    {
        var sb = new StringBuilder();

        sb.AppendLine("========================================");
        sb.AppendLine("       EMPLOYEE REPORT");
        sb.AppendLine("========================================");
        sb.AppendLine();

        sb.AppendLine($"Total Employees: {stats.TotalEmployees}");
        sb.AppendLine($"Average Salary: {stats.AverageSalary:C}");
        sb.AppendLine($"Min Salary: {stats.MinSalary:C} ({stats.MinSalaryEmployee})");
        sb.AppendLine($"Max Salary: {stats.MaxSalary:C} ({stats.MaxSalaryEmployee})");
        sb.AppendLine();

        sb.AppendLine("Department Breakdown:");
        sb.AppendLine("----------------------------------------");

        foreach (var dept in stats.Departments)
        {
            sb.AppendLine($"  {dept.Department}: {dept.EmployeeCount} employees, Avg: {dept.AverageSalary:C}");
        }

        sb.AppendLine("========================================");

        string report = sb.ToString();

        return report;
    }
}

