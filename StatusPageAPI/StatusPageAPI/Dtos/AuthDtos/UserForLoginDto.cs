using System.ComponentModel.DataAnnotations;

namespace StatusPageAPI.Dtos.AuthDtos
{
    public class UserForLoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}