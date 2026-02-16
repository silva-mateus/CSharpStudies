using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IX06_SOLID_Refactoring_ReportGenerator;
public interface IReportFormatter
{
    string Format(EmployeesStatistics stats);
}

