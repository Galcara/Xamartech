using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModelViewController.Models.ModelsAdministrator
{
    public class AdminInsertViewModel
    {
        [DisplayName("Nome")]
        [Required(ErrorMessage = "O nome deve ser informado.")]
        [StringLength(50, ErrorMessage = "O nome deve estar entre 3 e 50 caracteres.", MinimumLength = 3)]
        public string AdmName { get; set; }

        [DisplayName("CPF")]
        [Required(ErrorMessage = "O CPF deve ser informado")]
        [StringLength(11, ErrorMessage = "O CPF deve ter 11 caracteres.")]
        public string Cpf { get; set; }

        [DisplayName("Data de nascimento")]
        [Required(ErrorMessage = "A data de nascimento deve ser informada.")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [DisplayName("Telefone")]
        [Required(ErrorMessage = "O telefone deve ser informado.")]
        [StringLength(12, ErrorMessage = "O telefone deve conter 12 caracteres.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "O email deve ser informado.")]
        [StringLength(50, ErrorMessage = "O email deve estar entre 3 e 50 caracteres.", MinimumLength = 3)]
        public string Email { get; set; }

        public IFormFile PictureUpload { get; set; }

        [DisplayName("Senha")]
        [Required(ErrorMessage = "A senha deve ser informada.")]
        [StringLength(10, ErrorMessage = "A senha deve estar entre 4 e 10 caracteres.", MinimumLength = 4)]
        public string Passcode { get; set; }
    }
}