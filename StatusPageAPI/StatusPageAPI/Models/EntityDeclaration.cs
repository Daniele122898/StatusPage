using System;

namespace StatusPageAPI.Models
{
    public class EntityDeclaration
    {
        public Uri HealthEndpoint { get; set; }
        public bool Enabled { get; set; } = true;
    }
}