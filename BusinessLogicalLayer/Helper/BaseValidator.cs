using Common;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicalLayer
{
    public class BaseValidator<T>
    {
        private List<string> errors = new List<string>();

        public void AddError(string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                errors.Add(error);
            }
        }

        public virtual Response Validate(T item)
        {
            return CheckError();
        }

        private Response CheckError()
        {
            Response r = new Response();
            if (errors.Count != 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (string item in errors)
                {
                    builder.AppendLine(item);
                }
                r.Message = builder.ToString();
                r.Success = false;
                return r;
            }
            r.Success = true;
            return r;
        }
    }
}
