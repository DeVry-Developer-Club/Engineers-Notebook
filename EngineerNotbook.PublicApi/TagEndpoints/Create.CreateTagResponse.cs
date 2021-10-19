using System;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    public class CreateTagResponse : BaseResponse
    {
        public CreateTagResponse(Guid id) : base(id)
        {
        }

        public CreateTagResponse()
        {
        }

        public TagDto Tag { get; set; }
    }
}