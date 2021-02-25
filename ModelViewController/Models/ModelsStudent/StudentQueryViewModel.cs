using Metadata;
using System.Collections.Generic;
using System.ComponentModel;

namespace ModelViewController.Models.ModelsStudent
{
    public class StudentQueryViewModel
    {
        [DisplayName("ID")]
        public int ID { get; set; }

        [DisplayName("Nome")]
        public string StudentName { get; set; }

        [DisplayName("Matrícula")]
        public string Register { get; set; }

        [DisplayName("CPF")]
        public string Cpf { get; set; }

        [DisplayName("Telefone")]
        public string PhoneNumber { get; set; }

        [DisplayName("Turma")]
        public ICollection<Class> Classes { get; set; }
    }
}
