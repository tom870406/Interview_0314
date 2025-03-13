using System.ComponentModel.DataAnnotations;

namespace Interview_0314.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "請輸入帳號")]
        public string Username { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
