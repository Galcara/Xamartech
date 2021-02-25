using System;

#nullable disable

namespace Metadata
{
    [Serializable]
    public partial class Administrator
    {
        public int ID { get; set; }
        public string AdmName { get; set; }
        public string Cpf { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Passcode { get; set; }
        public bool Active { get; set; }
    }
}