namespace IX06_SOLID_Refactoring_ReportGenerator;

/// <summary>
/// THIS CLASS VIOLATES ALL SOLID PRINCIPLES.
/// Your task is to refactor it into clean, separated components.
/// After refactoring, rename this file to ReportGenerator_Original.cs and keep it for reference.
/// </summary>
public class ReportGenerator
{
    // Hardcoded CSV data -- SRP violation: data access mixed with business logic
    private readonly string _csvData;

    public ReportGenerator(string subdir, string csvFile)
    {
        var csvPath = Path.Combine(AppContext.BaseDirectory, "Data", "employees.csv");
        _csvData = File.ReadAllText(csvPath);
    }

    // This single method does EVERYTHING -- massive SRP violation
    public void GenerateReport(string format)
    {
        // ---- STEP 1: Parse CSV data (should be in a DataReader) ----
        var lines = _csvData.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var employees = new List<(string Name, string Department, decimal Salary, DateTime HireDate)>();

        for (int i = 1; i < lines.Length; i++) // skip header
        {
            var parts = lines[i].Trim().Split(',');
            if (parts.Length < 4) continue;

            var name = parts[0].Trim();
            var dept = parts[1].Trim();
            var salary = decimal.Parse(parts[2].Trim());
            var hireDate = DateTime.Parse(parts[3].Trim());
            employees.Add((name, dept, salary, hireDate));
        }

        // ---- STEP 2: Calculate statistics (should be in a StatisticsCalculator) ----
        var totalEmployees = employees.Count;
        decimal totalSalary = 0;
        decimal minSalary = decimal.MaxValue;
        decimal maxSalary = decimal.MinValue;
        string minSalaryEmployee = "";
        string maxSalaryEmployee = "";

        // Not using LINQ on purpose -- this is "legacy" code
        foreach (var emp in employees)
        {
            totalSalary += emp.Salary;
            if (emp.Salary < minSalary)
            {
                minSalary = emp.Salary;
                minSalaryEmployee = emp.Name;
            }
            if (emp.Salary > maxSalary)
            {
                maxSalary = emp.Salary;
                maxSalaryEmployee = emp.Name;
            }
        }
        var avgSalary = totalEmployees > 0 ? totalSalary / totalEmployees : 0;

        // Department breakdown -- manual grouping instead of LINQ
        var deptStats = new Dictionary<string, (int Count, decimal Total)>();
        foreach (var emp in employees)
        {
            if (!deptStats.ContainsKey(emp.Department))
                deptStats[emp.Department] = (0, 0);

            var current = deptStats[emp.Department];
            deptStats[emp.Department] = (current.Count + 1, current.Total + emp.Salary);
        }

        // ---- STEP 3: Format report (should be in a ReportFormatter) ----
        // OCP violation: adding a new format means modifying this method
        string report;

        if (format == "text")
        {
            report = "========================================\n";
            report += "       EMPLOYEE REPORT (Plain Text)     \n";
            report += "========================================\n\n";
            report += $"Total Employees: {totalEmployees}\n";
            report += $"Average Salary: {avgSalary:C}\n";
            report += $"Min Salary: {minSalary:C} ({minSalaryEmployee})\n";
            report += $"Max Salary: {maxSalary:C} ({maxSalaryEmployee})\n\n";
            report += "Department Breakdown:\n";
            report += "----------------------------------------\n";
            foreach (var dept in deptStats)
            {
                var deptAvg = dept.Value.Total / dept.Value.Count;
                report += $"  {dept.Key}: {dept.Value.Count} employees, Avg: {deptAvg:C}\n";
            }
            report += "========================================\n";
        }
        else if (format == "html")
        {
            report = "<html><body>";
            report += "<h1>Employee Report</h1>";
            report += "<table border='1'>";
            report += "<tr><th>Metric</th><th>Value</th></tr>";
            report += $"<tr><td>Total Employees</td><td>{totalEmployees}</td></tr>";
            report += $"<tr><td>Average Salary</td><td>{avgSalary:C}</td></tr>";
            report += $"<tr><td>Min Salary</td><td>{minSalary:C} ({minSalaryEmployee})</td></tr>";
            report += $"<tr><td>Max Salary</td><td>{maxSalary:C} ({maxSalaryEmployee})</td></tr>";
            report += "</table>";
            report += "<h2>Department Breakdown</h2><ul>";
            foreach (var dept in deptStats)
            {
                var deptAvg = dept.Value.Total / dept.Value.Count;
                report += $"<li>{dept.Key}: {dept.Value.Count} employees, Avg: {deptAvg:C}</li>";
            }
            report += "</ul></body></html>";
        }
        else
        {
            // No extensibility -- just throws
            throw new ArgumentException($"Unknown format: {format}");
        }

        // ---- STEP 4: Send report (should be in a ReportSender) ----
        // DIP violation: directly coupled to Console output
        Console.WriteLine("Sending report...");
        Console.WriteLine(report);
        Console.WriteLine("Report sent successfully!");

        // In the "original" code there was also an email attempt:
        // This is commented out but shows the coupling problem
        /*
        try
        {
            var smtpClient = new SmtpClient("smtp.company.com");
            var mailMessage = new MailMessage("reports@company.com", "manager@company.com");
            mailMessage.Subject = "Employee Report";
            mailMessage.Body = report;
            smtpClient.Send(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
        }
        */
    }
}
