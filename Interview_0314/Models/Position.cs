using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview_0314.Models
{
    public class Position
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "請輸入職務名稱")]
        public string Name { get; set; }

        [Required(ErrorMessage = "請選擇隊伍")]        
        [Range(1, int.MaxValue, ErrorMessage = "請選擇隊伍")]
        [ForeignKey("Team")]
        public int TeamId { get; set; }

        public virtual Team? Team { get; set; }
    }
}
