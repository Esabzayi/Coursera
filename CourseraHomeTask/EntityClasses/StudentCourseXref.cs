using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseraHomeTask.EntityClasses
{
    public class StudentCourseXref
    {
        public string StudentPin { get; set; }
        public int CourseId { get; set; }
        public DateTime? CompletionDate { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
