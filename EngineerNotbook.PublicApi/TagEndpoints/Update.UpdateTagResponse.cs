using System;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    public class UpdateTagResponse : BaseResponse
    {
        public UpdateTagResponse(Guid correlationId) : base(correlationId)
        {
        }

        public UpdateTagResponse()
        {
            
        }

        public TagDto Tag { get; set; }
    }
}