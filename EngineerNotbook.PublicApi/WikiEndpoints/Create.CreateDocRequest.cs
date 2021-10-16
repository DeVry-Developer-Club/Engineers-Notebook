using System.Collections.Generic;
namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    public class CreateDocRequest : BaseRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Contents { get; set; }
        public List<int> TagIds { get; set; }
    }
}