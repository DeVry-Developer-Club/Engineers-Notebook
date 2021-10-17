using System;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    public class GetByIdDocResponse : BaseResponse
    {
        public Documentation Doc { get; set; }

        public GetByIdDocResponse(Guid id) : base(id)
        {
            
        }
        
        public GetByIdDocResponse(){}
    }
}