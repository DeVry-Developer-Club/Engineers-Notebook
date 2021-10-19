using System;
using System.Collections.Generic;
namespace EngineerNotebook.PublicApi.TagEndpoints
{
    /// <summary>
    /// Response for retrieving all records of <see cref="Tags"/>
    /// </summary>
    public class ListTagsResponse : BaseResponse
    {
        public ListTagsResponse(Guid correlationId) : base(correlationId)
        {
        }

        public ListTagsResponse()
        {
            
        }

        public List<TagDto> Tags { get; set; } = new();
    }
}