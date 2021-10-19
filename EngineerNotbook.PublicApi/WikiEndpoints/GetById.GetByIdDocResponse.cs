using System;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    /// <summary>
    /// Response for retrieving a record of <see cref="Documentation"/> by ID
    /// </summary>
    public class GetByIdDocResponse : BaseResponse
    {
        public Documentation Doc { get; set; }

        public GetByIdDocResponse(Guid id) : base(id)
        {
            
        }
        
        public GetByIdDocResponse(){}
    }
}