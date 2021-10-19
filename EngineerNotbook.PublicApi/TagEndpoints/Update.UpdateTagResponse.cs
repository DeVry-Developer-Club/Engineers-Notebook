using System;
using EngineerNotebook.Shared.Endpoints;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    /// <summary>
    /// Response for updating a <see cref="Result"/>
    /// </summary>
    public class UpdateTagResponse : BaseResponse, IDtoResponse<TagDto>
    {
        public UpdateTagResponse(Guid correlationId) : base(correlationId)
        {
        }

        public UpdateTagResponse()
        {
            
        }

        public TagDto Result { get; set; }
    }
}