using System;
using System.Collections.Generic;

#nullable disable

namespace Metadata
{
    [Serializable]

    public partial class Student
    {
        public Student()
        {
            this.Lessons = new HashSet<Lesson>();
        }
        public int ID { get; set; }
        public string StudentName { get; set; }
        public string Cpf { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Register { get; set; }
        public string Passcode { get; set; }
        public bool Active { get; set; }
        public int ClassID { get; set; }
        public virtual Class Class { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
        public virtual ICollection<Presence> Presences { get; set; }
    }
}