using Metadata.Enums;
using System;
using System.Collections.Generic;

#nullable disable

namespace Metadata
{
    [Serializable]

    public partial class Class
    {
        public Class()
        {
            this.Students = new HashSet<Student>();
        }
        public int ID { get; set; }
        public string ClassName { get; set; }
        public Shift ClassShift { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}