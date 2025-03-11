using System.ComponentModel.DataAnnotations;

namespace Interview_0314.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "請輸入隊伍名稱")]
        [StringLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "請輸入隊伍城市")]
        public string City { get; set; }
        [Required(ErrorMessage = "請輸入隊伍主場")]
        public string Stadium { get; set; }
        [Required(ErrorMessage = "請輸入成立年份")] // 這行可避免 null 值
        [Range(1900, 2100, ErrorMessage = "成立年份必須在 1900 到 2100 之間")]
        public int? FoundedYear { get; set; }
    }
}
