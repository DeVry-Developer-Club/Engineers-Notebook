using System;
using EngineerNotebook.Shared.Endpoints;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    /// <summary>
    /// Response for getting a record of <see cref="Result"/> with a given Tag
    /// </summary>
    public class GetByIdTagResponse : BaseResponse, IDtoResponse<TagDto>
    {
        public GetByIdTagResponse()
        {
            
        }

        public GetByIdTagResponse(Guid correlationId) : base(correlationId)
        {
            
        }
        
        public TagDto Result { get; set; } = new();
    }
}