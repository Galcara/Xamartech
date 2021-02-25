using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModelViewController.Models.ModelsTeacher
{
    public class TeacherInsertViewModel
    {
        [DisplayName("Nome do professor")]
        [Required(ErrorMessage = "Nome deve ser informado.")]
        public string TeacherName { get; set; }

        [DisplayName("CPF")]
        [Required(ErrorMessage = "O CPF deve ser informado")]
        [StringLength(11, ErrorMessage = "O CPF deve ter 11 caracteres.")]
        public string Cpf { get; set; }

        [DisplayName("Data de nascimento")]
        [Required(ErrorMessage = "A data de nascimento deve ser informada.")]
        [DataType(DataType.Date)]

        public DateTime BirthDate { get; set; }

        [DisplayName("Número de telefone")]
        [Required(ErrorMessage = "Número de telefone deve ser informado.")]
        public string PhoneNumber { get; set; }

        public IFormFile PictureUpload { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Insira uma foto")]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("Matéria")]
        [Required(ErrorMessage = "Selecione uma matéria")]
        public List<int> Subjects { get; set; }
    }
}
