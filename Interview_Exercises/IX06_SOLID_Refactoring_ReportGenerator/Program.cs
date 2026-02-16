using IX06_SOLID_Refactoring_ReportGenerator;

var format = args.Length >0 ? args[0].ToLower() : "text";
var csvPath = Path.Combine(AppContext.BaseDirectory, "Data", "employees.csv");

IDataReader reader = new CsvDataReader(csvPath);
IStatisticsCalculator calculator = new EmployeeStatisticsCalculator();
IReportSender sender = new ConsoleReportSender();
IReportFormatter formatter = format switch
{
    "text" => new PlainTextReportFormatter(),
    "html" => new HtmlReportFormatter(),
    _ => throw new ArgumentException($"Unknown format: {format}")
};

var service = new ReportService(reader, calculator, formatter, sender);
service.GenerateAndSendReport();