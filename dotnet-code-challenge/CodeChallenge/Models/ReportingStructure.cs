namespace CodeChallenge.Models
{
    public class ReportingStructure
    {
        public ReportingStructure(Employee Employee, int NumberOfReports) {
            this.Employee = Employee;
            this.NumberOfReports = NumberOfReports;
        }

        public Employee Employee { get; set; }
        public int NumberOfReports { get; set; }
    }
}
