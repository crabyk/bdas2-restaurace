using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDAS2_Restaurace.Model
{
    public abstract class PropertyValidateModel : IDataErrorInfo
    {
        public string Error { get { return null; } }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                var validationResults = new List<ValidationResult>();

                if (Validator.TryValidateProperty(
                    GetType().GetProperty(columnName).GetValue(this),
                    new ValidationContext(this) { MemberName = columnName },
                    validationResults
                    ))
                {
                    return null;
                }

                return validationResults.First().ErrorMessage;
            }
        }

        public static bool Validate<T>(T obj, out ICollection<ValidationResult> results)
        {
            results = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
        }


        public static bool Validate<T>(T obj)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
        }

    }
}
