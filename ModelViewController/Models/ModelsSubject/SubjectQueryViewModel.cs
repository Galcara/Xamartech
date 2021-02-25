using System.ComponentModel;

namespace ModelViewController.Models.ModelsSubject
{
    public class SubjectQueryViewModel
    {
        [DisplayName("ID")]
        public int ID { get; set; }

        [DisplayName("Matéria")]
        public string SubjectName { get; set; }

    }
}
