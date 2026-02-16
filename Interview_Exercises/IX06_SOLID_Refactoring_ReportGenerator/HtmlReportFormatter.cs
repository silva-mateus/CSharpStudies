using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IX06_SOLID_Refactoring_ReportGenerator;
public class HtmlReportFormatter : IReportFormatter
{
    public string Format(EmployeesStatistics stats)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<html><body>");
        sb.AppendLine("<h1>Employee Report</h1>");
        sb.AppendLine("<table border='1'>");
        sb.AppendLine("<tr><th>Metric</th><th>Value</th></tr>");
        sb.AppendLine($"<tr><td>Total Employees</td><td>{stats.TotalEmployees}</td></tr>");
        sb.AppendLine($"<tr><td>Average Salary</td><td>{stats.AverageSalary:C}</td></tr>");
        sb.AppendLine($"<tr><td>Min Salary</td><td>{stats.MinSalary:C} ({stats.MinSalaryEmployee})</td></tr>");
        sb.AppendLine($"<tr><td>Max Salary</td><td>{stats.MaxSalary:C} ({stats.MaxSalaryEmployee})</td></tr>");
        sb.AppendLine("</table>");
        sb.AppendLine("<h2>Department Breakdown</h2><ul>");
        foreach (var dept in stats.Departments)
        {
            sb.AppendLine($"<li>{dept.Department}: {dept.EmployeeCount} employees, Avg: {dept.AverageSalary:C}</li>");
        }

        sb.AppendLine("</ul></body></html>");

        var report = sb.ToString();
        return report;
    }
}

