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
        
        public string Description { get; set; }
        
        public string Error { get; set; }

        public List<EntityStatus> SubEntities { get; set; }

        public bool IsCategory => SubEntities?.Count > 0;

        public EntityStatus SetOverrides(EntityDeclaration e)
        {
            this.Identifier = string.IsNullOrWhiteSpace(e.Identifier) ? this.Identifier : e.Identifier;
            this.Description = string.IsNullOrWhiteSpace(e.Description) ? this.Description : e.Description;
            return this;
        }
    }
}