using System.Globalization;
using CourseraHomeTask;
using CourseraHomeTask.EntityClasses;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;


class Program
{
    static void Main()
    {
        TakingInputs();
    }
    static void TakingInputs()
    {
        Console.Write("Enter the comma-separated list of personal identifiers (PIN) of the students to be included in the report (press Enter for all students): ");
        string pinInput = Console.ReadLine();
        string[] pinList = string.IsNullOrWhiteSpace(pinInput) ? new string[0] : pinInput.Split(',');

        Console.Write("Enter the required minimum credit: ");
        int MinimumCredit = int.Parse(Console.ReadLine());

        Console.Write("Enter the start date of the time period for which the students should have collected the requested credit (MM/dd/yyyy): ");
        DateTime StartDate = DateTime.ParseExact(Console.ReadLine(), "MM/dd/yyyy", CultureInfo.InvariantCulture);

        Console.Write("Enter the end date of the time period for which the students should have collected the requested credit (MM/dd/yyyy): ");
        DateTime EndDate = DateTime.ParseExact(Console.ReadLine(), "MM/dd/yyyy", CultureInfo.InvariantCulture);

        Console.Write("Enter the output format (csv or press Enter): ");
        string OutputFormat = Console.ReadLine();

        Console.Write("Enter the path to the directory where the reports will be saved: ");
        string OutputDirectory = Console.ReadLine();

        using (var dbContext = new CourseraEntities())
        {
            IQueryable<Student> studentsQuery = dbContext.Students
                .Include(s => s.CompletedCourses)
                .ThenInclude(c => c.Course)
                .ThenInclude(course => course.Instructor);

            if (pinList.Length > 0)
            {
                studentsQuery = studentsQuery.Where(s => pinList.Contains(s.PIN));
            }

            List<Student> EligibleStudents = studentsQuery
                .Where(s => s.CompletedCourses
                    .Where(c => c.CompletionDate >= StartDate && c.CompletionDate <= EndDate)
                    .Sum(c => c.Course.Credit) >= MinimumCredit)
                .ToList();

            SaveReports(EligibleStudents, OutputDirectory , OutputFormat);
        }
    }
    static void SaveReports(List<Student> students, string outputDirectory, string outputFormat)
    {
       
        string csvReportPath = Path.Combine(outputDirectory, "report.csv");

        if (outputFormat == "csv")
        {
            CsvReport(students, csvReportPath);
            Console.WriteLine($"CSV report saved to: {csvReportPath}");
        }
        else if (string.IsNullOrEmpty(outputFormat))
        {
            CsvReport(students, csvReportPath);
            Console.WriteLine($"CSV report saved to: {csvReportPath}");
        }
    }
    static void CsvReport(List<Student> students, string filePath)
    {
        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            csv.WriteField("Student Name");
            csv.WriteField("Total Credit");
            csv.WriteField("Course Name");
            csv.WriteField("Total Time");
            csv.WriteField("Credit");
            csv.WriteField("Instructor Name");
            csv.NextRecord();

            foreach (var student in students)
            {
                csv.WriteField($"{student.FirstName} {student.LastName}");
                csv.WriteField(student.CompletedCourses.Sum(c => c.Course.Credit));
                csv.NextRecord();

                foreach (var course in student.CompletedCourses)
                {
                    csv.WriteField(""); 
                    csv.WriteField(""); 
                    csv.WriteField(course.Course.Name);
                    csv.WriteField(course.Course.TotalTime);
                    csv.WriteField(course.Course.Credit);
                    csv.WriteField($"{course.Course.Instructor.FirstName} {course.Course.Instructor.LastName}");
                    csv.NextRecord();
                }
            }
        }
    } 
}
