using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.web.Models
{
    public class OrderDetailDto
    {
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }
        
        public int ProductId { get; set; }
       
        public ProductDto? Product { get; set; }
        public int Count { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}
