using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLogicalLayer
{
    public static class StringExtensions
    {
        public static string IsValidName(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "Nome deve ser informado.";
            }
            if (name.Length < 3 || name.Length > 50)
            {
                return "Nome deve ter entre 3 e 50 caracteres.";
            }
            return "";
        }
        public static string IsValidCPF(this string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return "CPF deve conter 11 caracteres.";
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf += digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito += resto.ToString();
            bool val = cpf.EndsWith(digito);
            if (val)
            {
                return "";
            }
            return "CPF inválido.";
        }
        public static string IsValidPhoneNumber(this string phonenumber)
        {
            if (string.IsNullOrWhiteSpace(phonenumber))
            {
                return "Telefone inválido.";
            }
            else if (phonenumber.Length > 11 && phonenumber.Length < 9)
            {
                return "Telefone inválido.";
            }
            return "";
        }
        public static string IsValidEmail(this string email)
        {
            Regex regex = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");
            if (regex.IsMatch(email))
            {
                return "";
            }
            return "Email inválido";
        }
        public static string IsValidPasscode(this string passcode)

        {
            if (string.IsNullOrWhiteSpace(passcode))
            {
                return "Senha inválida.";
            }
            if (passcode.Length < 4 || passcode.Length > 11)
            {
                return "A senha deve conter entre 4 e 10 caracteres";
            }
            return "";
        }
        public static string RemoveMaskCPF(this string cpf)
        {
            cpf = cpf.Replace(".", "").Replace("-", "");
            return cpf;
        }
        public static string RemoveMaskPhoneNumber(this string phonenumber)
        {
            phonenumber = phonenumber.Replace("(", "").Replace(")", "").Replace("-", "");
            return phonenumber;
        }
        public static string EncryptPassword(this string passcode)
        {
            var encodedValue = Encoding.UTF8.GetBytes(passcode);
            var encryptedPassword = MD5.Create().ComputeHash(encodedValue);

            var sb = new StringBuilder();
            foreach (var caracter in encryptedPassword)
            {
                sb.Append(caracter.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
