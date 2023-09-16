using System.ComponentModel.DataAnnotations;

namespace Mango.web.Util
{
    public class AllowedExtensionsAtribute :ValidationAttribute

    {
        private readonly string[] _extensions;
        public AllowedExtensionsAtribute(string[] extensions)
        {
            _extensions = extensions;   
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extensions = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extensions.ToLower()))
                {
                    return new ValidationResult("This photo is not allowed!!!");
                }
            }
            return ValidationResult.Success;
        }
    }
}
