using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IX06_SOLID_Refactoring_ReportGenerator;
public class ConsoleReportSender : IReportSender
{
    public void Send(string report)
    {
        Console.WriteLine(report);
    }
}

