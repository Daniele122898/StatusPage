using System.ComponentModel.DataAnnotations;
using StatusPageAPI.Models.Enums;

namespace StatusPageAPI.Dtos.StatusDtos
{
    public class SetSpecialNoticeDto
    {
        [Required]
        public Status Status { get; set; }
        
        [Required]
        public string Notice { get; set; }
    }
}