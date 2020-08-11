using System.Collections.Generic;
using StatusPageAPI.Models;

namespace StatusPageAPI.Services
{
    public class StatusService
    {
        private readonly List<EntityStatus> _entityCache = new List<EntityStatus>();

        /// <summary>
        /// Get list of entity status collection as a read only list so it cannot be mutated outside of cache
        /// </summary>
        public IReadOnlyCollection<EntityStatus> GetStatuses() => _entityCache.AsReadOnly();

        /// <summary>
        /// Set the cached list to this list for other services to grab :)
        /// </summary>
        public void SetStatuses(IEnumerable<EntityStatus> statuses)
        {
            this._entityCache.Clear();
            this._entityCache.AddRange(statuses);
        }
    }
}