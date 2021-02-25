using Metadata.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModelViewController.Models.ModelsClass
{
    public class ClassInsertViewModel
    {
        [DisplayName("Nome da turma")]
        [Required(ErrorMessage = "O nome da turma deve ser informado.")]
        public string ClassName { get; set; }

        [DisplayName("Turno")]
        [Required(ErrorMessage = "O turno da turma deve ser informado.")]
        public Shift ClassShift { get; set; }
    }
}
