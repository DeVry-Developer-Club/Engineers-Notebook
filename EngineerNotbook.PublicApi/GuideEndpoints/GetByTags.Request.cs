using System.Collections.Generic;
using EngineerNotebook.PublicApi.TagEndpoints;

namespace EngineerNotebook.PublicApi.Guide
{
    public class GetByTagsRequest : BaseRequest
    {
        public List<int> TagIds { get; set; }
    }
}