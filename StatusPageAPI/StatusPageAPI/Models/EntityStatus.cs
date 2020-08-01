using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StatusPageAPI.Models.Enums;

namespace StatusPageAPI.Models
{
    public class EntityStatus
    {
        [Required]
        public string Identifier { get; set; }

        [Required]
        public Status Status { get; set; }
        
        public string Error { get; set; }

        public List<EntityStatus> SubEntities { get; set; }

        public bool IsCategory => SubEntities?.Count > 0;
    }
}