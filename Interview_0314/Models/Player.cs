using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Interview_0314.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "請輸入球員姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "請選擇隊伍")]
        public int TeamId { get; set; }

        [Required(ErrorMessage = "請選擇職務")]
        public int PositionId { get; set; }

        public string? PhotoPath { get; set; }  // 存儲圖片路徑

        // 外鍵關聯
        [ForeignKey("TeamId")]
        public virtual Team? Team { get; set; }

        [ForeignKey("PositionId")]
        public virtual Position? Position { get; set; }
    }
}
