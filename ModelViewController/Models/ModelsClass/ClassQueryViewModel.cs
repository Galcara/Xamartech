using Metadata.Enums;
using System.ComponentModel;

namespace ModelViewController.Models.ModelsClass
{
    public class ClassQueryViewModel
    {
        [DisplayName("ID")]
        public int ID { get; set; }

        [DisplayName("Turma")]
        public string ClassName { get; set; }

        [DisplayName("Turno")]
        public Shift ClassShift { get; set; }
    }
}
