using System;
using System.ComponentModel;

namespace ModelViewController.Models.ModelsAdministrator
{
    public class AdminQueryViewModel
    {
        [DisplayName("ID")]
        public int ID { get; set; }

        [DisplayName("Nome")]
        public string AdmName { get; set; }

        [DisplayName("CPF")]
        public string Cpf { get; set; }

        [DisplayName("Data de nascimento")]
        public DateTime BirthDate { get; set; }

        [DisplayName("Telefone")]
        public string PhoneNumber { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }
    }
}