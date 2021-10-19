using System;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    public class DeleteTagResponse : BaseResponse
    {
        public DeleteTagResponse()
        {
        }

        public DeleteTagResponse(Guid correlationId) : base(correlationId)
        {
        }

        public string Status { get; set; } = "Deleted";
    }
}