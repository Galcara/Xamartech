using System;
using System.Text;

namespace BusinessLogicalLayer.Interfaces
{
    public class RandomRegisterWithGuid : IGenerateRegister
    {
        public string GenerateRandonRegister()
        {
            Guid g = Guid.NewGuid();
            string withoutHifen = g.ToString().Replace("-", "");
            StringBuilder numeroMatricula = new StringBuilder();
            for (int j = 0; j < withoutHifen.Length; j++)
            {
                if (char.IsLetter(withoutHifen[j]))
                {
                    numeroMatricula.Append((int)withoutHifen[j]);
                }
                else
                {
                    numeroMatricula.Append(withoutHifen[j]);
                }
            }
            return numeroMatricula.ToString(0, 8);
        }
    }
}
