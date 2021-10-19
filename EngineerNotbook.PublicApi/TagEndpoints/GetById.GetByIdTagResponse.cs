using System;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    /// <summary>
    /// Response for getting a record of <see cref="Tag"/> with a given Tag
    /// </summary>
    public class GetByIdTagResponse : BaseResponse
    {
        public GetByIdTagResponse()
        {
            
        }

        public GetByIdTagResponse(Guid correlationId) : base(correlationId)
        {
            
        }
        
        public Tag Tag { get; set; } = new();
    }
}