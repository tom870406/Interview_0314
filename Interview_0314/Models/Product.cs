#region


#endregion

using System.ComponentModel.DataAnnotations;
namespace Interview_0314.Models
{
    public class Product
    {
        [Key]  // 設為主鍵
        public int Id { get; set; }

        [Required]  // 必填
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Description { get; set; }
    }
}
