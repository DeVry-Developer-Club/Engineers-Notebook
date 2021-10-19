using System;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    /// <summary>
    /// Response for updating a <see cref="Tag"/>
    /// </summary>
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