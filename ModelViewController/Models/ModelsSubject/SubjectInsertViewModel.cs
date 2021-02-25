using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModelViewController.Models.ModelsSubject
{
    public class SubjectInsertViewModel
    {
        [DisplayName("Nome da matéria")]
        [Required(ErrorMessage = "O nome da matéria deve ser informado.")]
        public string SubjectName { get; set; }
    }
}
