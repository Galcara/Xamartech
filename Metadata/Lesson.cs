using Metadata.Enums;
using System;
using System.Collections.Generic;

#nullable disable

namespace Metadata
{
    [Serializable]

    public partial class Lesson
    {
        public Lesson()
        {
            this.Students = new HashSet<Student>();
        }

        public override string ToString()
        {
            return this.date.ToString("dd/MM/yyyy - HH:mm:ss");
        }

        public Lesson(Teacher teacher, Class @class, DayOfWeek dayOfWeek)
        {
            this.Teacher = teacher;
            this.Class = @class;
            this.Students = new HashSet<Student>();
            this.LessonDate = dayOfWeek;

        }

        public int ID { get; set; }
        public DayOfWeek LessonDate { get; set; }
        public DateTime date { get; set; }
        public int ClassID { get; set; }
        public virtual Class Class { get; set; }
        public Shift Shift { get; set; }
        public LessonOrder Order { get; set; }
        public int TeacherID { get; set; }
        public virtual Teacher Teacher { get; set; }
        public int SubjectID { get; set; }
        public virtual Subject Subject { get; set; }

        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Presence> Presences { get; set; }

    }

}