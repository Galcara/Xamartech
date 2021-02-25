using System;

namespace Metadata
{
    [Serializable]

    public class Presence
    {
        public int ID { get; set; }
        public int LessonID { get; set; }
        public virtual Lesson Lesson { get; set; }
        public int StudentID { get; set; }
        public virtual Student Student { get; set; }
        public bool Attendance { get; set; }
    }
}
