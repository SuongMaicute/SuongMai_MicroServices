
using System.ComponentModel.DataAnnotations;

namespace Mango.web.Models
{
	public class ProductDto
	{
		
		public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Range(1, 1000)]
        public double Price { get; set; }
		public string Description { get; set; }
		public string CategoryName { get; set; }
		public string? ImageUrl { get; set; }
		public string? ImageLocalPath { get; set; }
		[Range(1,100)]
		public int Count { get; set; } = 1;
		public IFormFile? Image { get; set; }
	}
}
