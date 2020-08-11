using System;

namespace StatusPageAPI.Models
{
    public class EntityDeclaration
    {
        public string Identifier { get; set; }
        public Uri HealthEndpoint { get; set; }
        public bool Enabled { get; set; } = true;
    }
}