using System;
using System.Collections.Generic;

#nullable disable

namespace Metadata
{
    [Serializable]

    public partial class Subject
    {
        public Subject()
        {
            this.Teachers = new HashSet<Teacher>();
        }
        public int ID { get; set; }
        public string SubjectName { get; set; }
        public bool Active { get; set; }
        public int Frequency { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}