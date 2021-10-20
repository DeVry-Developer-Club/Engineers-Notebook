using System;
using Ardalis.ApiEndpoints;

namespace EngineerNotebook.PublicApi
{
    /// <summary>
    /// Building block for responses using <see cref="BaseAsyncEndpoint"/>
    /// </summary>
    public abstract class BaseResponse : BaseMessage
    {
        public BaseResponse(Guid correlationId) : base()
        {
            _correlationId = correlationId;
        }

        public BaseResponse()
        {
        }
    }
}