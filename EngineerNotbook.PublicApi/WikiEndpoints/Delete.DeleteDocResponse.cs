using System;
namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    public class DeleteDocResponse : BaseResponse
    {
        public DeleteDocResponse(Guid id) : base(id)
        {
            
        }

        public DeleteDocResponse()
        {
            
        }

        public string Status { get; set; } = "Deleted";
    }
}