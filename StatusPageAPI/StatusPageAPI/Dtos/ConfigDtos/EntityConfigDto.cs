using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using StatusPageAPI.Models;

namespace StatusPageAPI.Dtos.ConfigDtos
{
    public class EntityConfigDto
    {
        [Required]
        public string Identifier { get; set; }
        
        public string Description { get; set; }

        public Uri HealthEndpoint { get; set; }

        public List<EntityConfigDto> SubEntities { get; set; }

        public bool Enabled { get; set; } = true;
        public bool IsCategory { get; set; } = false;

        public EntityDeclaration ToDeclaration()
            => new EntityDeclaration()
            {
                Identifier = Identifier,
                Description = Description,
                HealthEndpoint = HealthEndpoint,
                SubEntities = SubEntities?.Select(x => x.ToDeclaration()).ToList(),
                Enabled = Enabled,
                IsCategory = IsCategory
            };
    }
}