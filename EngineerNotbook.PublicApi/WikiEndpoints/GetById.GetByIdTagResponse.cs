using System;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
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