using System;
using EngineerNotebook.Shared.Endpoints;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    /// <summary>
    /// Response for creating a new <see cref="Documentation"/>
    /// </summary>
    public class CreateDocResponse : BaseResponse, IDtoResponse<DocDto>
    {
        public CreateDocResponse(Guid id) : base(id)
        {
        }

        public CreateDocResponse()
        {
        }

        public DocDto Result { get; set; }
    }
}