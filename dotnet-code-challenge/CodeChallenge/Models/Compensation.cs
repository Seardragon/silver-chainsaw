using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        public String EmployeeId { get; set; }
        public Double Salary { get; set; }
        public DateTime EffectiveDate { get; set; }

        [NotMapped]
        public Employee Employee { get; set; }
    }
}
