using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiEfCodeFirst.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")] public Product Product { get; set; }
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")] public User User { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
