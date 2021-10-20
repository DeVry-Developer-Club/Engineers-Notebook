using System;
using EngineerNotebook.Shared.Endpoints;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    /// <summary>
    /// Response for retrieving a record of <see cref="Documentation"/> by ID
    /// </summary>
    public class GetByIdDocResponse : BaseResponse, IDtoResponse<DocDto>
    {
        public DocDto Result { get; set; }

        public GetByIdDocResponse(Guid id) : base(id)
        {
            
        }
        
        public GetByIdDocResponse(){}
    }
}