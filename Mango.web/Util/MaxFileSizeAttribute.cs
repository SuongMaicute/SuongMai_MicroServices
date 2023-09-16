using System.ComponentModel.DataAnnotations;

namespace Mango.web.Util
{
    public class MaxFileSizeAttribute :ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int fileSize)
        {
            _maxFileSize = fileSize;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length> (_maxFileSize*1024*1024))
                {
                    return new ValidationResult($"Maximun allowed file size is {_maxFileSize} MB.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
