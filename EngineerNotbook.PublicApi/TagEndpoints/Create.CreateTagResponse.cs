using System;
using EngineerNotebook.Shared.Endpoints;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    /// <summary>
    /// Response for creating a new record of <see cref="Result"/>
    /// </summary>
    public class CreateTagResponse : BaseResponse, IDtoResponse<TagDto>
    {
        public CreateTagResponse(Guid id) : base(id)
        {
        }

        public CreateTagResponse()
        {
        }

        public TagDto Result { get; set; }
    }
}