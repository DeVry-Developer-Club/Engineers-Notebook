using System;

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    public class UpdateDocResponse : BaseResponse
    {
        public UpdateDocResponse(Guid id) : base(id)
        {
            
        }

        public UpdateDocResponse()
        {
            
        }

        public DocDto Doc { get; set; }
    }
}