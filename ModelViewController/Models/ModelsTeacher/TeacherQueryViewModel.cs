using Metadata;
using System.Collections.Generic;
using System.ComponentModel;

namespace ModelViewController.Models.ModelsTeacher
{
    public class TeacherQueryViewModel
    {
        [DisplayName("ID")]
        public int ID { get; set; }

        [DisplayName("Nome")]
        public string TeacherName { get; set; }

        [DisplayName("CPF")]
        public string Cpf { get; set; }

        [DisplayName("Telefone")]
        public string PhoneNumber { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Matéria")]
        public ICollection<Subject> Subjects { get; set; }

    }
}
