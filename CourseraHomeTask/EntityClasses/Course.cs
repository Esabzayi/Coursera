using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseraHomeTask.EntityClasses
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }
        public int TotalTime { get; set; }
        public int Credit { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}
