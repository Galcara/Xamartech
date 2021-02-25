using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModelViewController.Models.ModelsStudent
{
    public class StudentInsertViewModel
    {
        [DisplayName("Nome do estudante")]
        [Required(ErrorMessage = "O nome do estudante deve ser informado.")]
        public string StudentName { get; set; }

        [DisplayName("CPF")]
        [Required(ErrorMessage = "O CPF deve ser informado")]
        [StringLength(11, ErrorMessage = "O CPF deve ter 11 caracteres.")]
        public string Cpf { get; set; }

        [DisplayName("Data de nascimento")]
        [Required(ErrorMessage = "A data nascimento deve ser informada.")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [DisplayName("Número de telefone")]
        [Required(ErrorMessage = "O número de telefone deve ser informado.")]
        public string PhoneNumber { get; set; }

        [DisplayName("Foto")]
        [Required(ErrorMessage = "Insira uma foto")]
        public IFormFile PictureUpload { get; set; }

        [DisplayName("Turma")]
        [Required(ErrorMessage = "Escolha uma turma")]
        public int ClassID { get; set; }

    }
}
