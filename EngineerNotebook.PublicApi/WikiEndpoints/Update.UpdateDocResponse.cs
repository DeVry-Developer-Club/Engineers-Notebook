using System;
using EngineerNotebook.Shared.Endpoints;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    /// <summary>
    /// Response for updating <see cref="Documentation"/>
    /// </summary>
    public class UpdateDocResponse : BaseResponse, IDtoResponse<DocDto>
    {
        public UpdateDocResponse(Guid id) : base(id)
        {
            
        }

        public UpdateDocResponse()
        {
            
        }

        public DocDto Result { get; set; }
    }
}