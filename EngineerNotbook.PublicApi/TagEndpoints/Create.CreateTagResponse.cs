using System;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    /// <summary>
    /// Response for creating a new record of <see cref="Tag"/>
    /// </summary>
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