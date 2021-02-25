using System;
using System.Text;

namespace BusinessLogicalLayer.Interfaces
{
    public class RandomRegisterWIthNumbers : IGenerateRegister
    {
        public string GenerateRandonRegister()
        {
            Random random = new Random();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                int randomRegister = random.Next(1, 99);
                string temp = randomRegister.ToString();
                builder.AppendLine(temp);
            }
            string register = builder.ToString();
            return register;
        }
    }
}
