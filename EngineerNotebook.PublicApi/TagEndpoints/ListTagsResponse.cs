using System;
using System.Collections.Generic;
using EngineerNotebook.Shared.Endpoints;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    /// <summary>
    /// Response for retrieving all records of <see cref="Results"/>
    /// </summary>
    public class ListTagsResponse : BaseResponse, IDtoResponseCollection<TagDto>
    {
        public ListTagsResponse(Guid correlationId) : base(correlationId)
        {
        }

        public ListTagsResponse()
        {
            
        }

        public List<TagDto> Results { get; set; } = new();
    }
}