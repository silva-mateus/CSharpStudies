using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IX06_SOLID_Refactoring_ReportGenerator;
public class ReportService
{

    private readonly IDataReader _dataReader;
    private readonly IStatisticsCalculator _statisticsCalculator;
    private readonly IReportFormatter _formatter;
    private readonly IReportSender _reportSender;

    public ReportService(IDataReader dataReader, IStatisticsCalculator statisticsCalculator, IReportFormatter formatter, IReportSender reportSender)
    {
        _dataReader = dataReader;
        _statisticsCalculator = statisticsCalculator;
        _formatter = formatter;
        _reportSender = reportSender;
    }

    public void GenerateAndSendReport()
    {
        var employees = _dataReader.Read();
        var employeesStatistics = _statisticsCalculator.Calculate(employees);

        var report = _formatter.Format(employeesStatistics);

        _reportSender.Send(report);
    }
}

