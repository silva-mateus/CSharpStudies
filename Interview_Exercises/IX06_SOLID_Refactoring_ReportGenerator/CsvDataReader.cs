using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace IX06_SOLID_Refactoring_ReportGenerator;

public class CsvDataReader : IDataReader
{
    private readonly string _filePath;

    public CsvDataReader(string filePath)
    {
        _filePath = filePath; 
    }

    public List<Employee> Read()
    {
        if (!File.Exists(_filePath)) 
            throw new FileNotFoundException($"CSV file not found: {_filePath}");

        var lines = File.ReadAllLines(_filePath);
        var employees = new List<Employee>();

        if (lines.Length <= 1)
            return employees;

        for (int i = 1; i < lines.Length; i++)
        {
            var parts = lines[i].Trim().Split(',');
            if (parts.Length < 4) continue;

            var name = parts[0].Trim();
            var department = parts[1].Trim();

            if (!decimal.TryParse(parts[2].Trim(), out var salary))
                continue;
            if (!DateTime.TryParse(parts[3].Trim(), out var hireDate))
                continue;

            employees.Add(new Employee(name, department, salary, hireDate));
        }

        return employees;

    }

}

