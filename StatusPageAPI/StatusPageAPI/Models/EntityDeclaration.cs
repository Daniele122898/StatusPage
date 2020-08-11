using System;
using System.Collections.Generic;

namespace StatusPageAPI.Models
{
    public class EntityDeclaration
    {
        public string Identifier { get; set; }
        public string Description { get; set; }
        public Uri HealthEndpoint { get; set; }

        public List<EntityDeclaration> SubEntities { get; set; }

        public bool Enabled { get; set; } = true;
        public bool IsCategory { get; set; } = false;
    }
}