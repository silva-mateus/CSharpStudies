using IX06_SOLID_Refactoring_ReportGenerator;

// Current usage (before refactoring):
// The monolithic ReportGenerator handles everything.
var generator = new ReportGenerator();

Console.WriteLine("--- Plain Text Report ---");
generator.GenerateReport("text");

Console.WriteLine("\n--- HTML Report ---");
generator.GenerateReport("html");

// After refactoring, this Program.cs should become the composition root:
// 1. Create IDataReader (CsvDataReader)
// 2. Create IStatisticsCalculator (EmployeeStatisticsCalculator)
// 3. Create IReportFormatter (PlainTextReportFormatter or HtmlReportFormatter)
// 4. Create IReportSender (ConsoleReportSender)
// 5. Create ReportService with all dependencies injected
// 6. Call ReportService.GenerateAndSendReport()
