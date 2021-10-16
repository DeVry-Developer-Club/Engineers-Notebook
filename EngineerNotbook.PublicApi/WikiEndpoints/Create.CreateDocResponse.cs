using System;
namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    public class CreateDocResponse : BaseResponse
    {
        public CreateDocResponse(Guid id) : base(id)
        {
        }

        public CreateDocResponse()
        {
        }

        public DocDto Doc { get; set; }
    }
}