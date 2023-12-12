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

            if (!Validator.TryValidateObject(obj, new ValidationContext(obj), results, true))
                return false;

            foreach (var prop in TypeDescriptor.GetProperties(obj).Cast<PropertyDescriptor>())
            {
                try
                {
                    // Attempt to get the property value to trigger any conversion errors
                    var value = prop.GetValue(obj);
                }
                catch (FormatException ex)
                {
                    // If a conversion error occurs, add it to the validation results
                    results.Add(new ValidationResult($"Conversion error: {ex.Message}", new[] { prop.Name }));
                }
                catch (Exception ex)
                {
                    // Handle other exceptions if needed
                    results.Add(new ValidationResult($"Error: {ex.Message}", new[] { prop.Name }));
                }
            }

            return results.Count == 0;
        }

    }
}
