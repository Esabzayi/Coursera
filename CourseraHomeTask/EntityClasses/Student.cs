using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseraHomeTask.EntityClasses
{
    public class Student
    {
        public string PIN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<StudentCourseXref> CompletedCourses { get; set; }
    }
}
