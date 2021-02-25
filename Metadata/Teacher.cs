using System;
using System.Collections.Generic;

#nullable disable

namespace Metadata
{
    [Serializable]

    public partial class Teacher
    {
        public Teacher()
        {
            this.Subjects = new HashSet<Subject>();
        }
        public int ID { get; set; }
        public string TeacherName { get; set; }
        public string Cpf { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Passcode { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}