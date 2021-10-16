using System;

namespace EngineerNotebook.PublicApi
{
    /// <summary>
    /// Base class used by API requests
    /// </summary>
    public abstract class BaseMessage
    {
        /// <summary>
        /// Unique identifier used for logging
        /// </summary>
        protected Guid _correlationId = Guid.NewGuid();
        public Guid CorrelationId() => _correlationId;
    }
}