using System.Collections.Generic;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    /// <summary>
    /// Request for creating a new <see cref="Documentation"/>
    /// </summary>
    public class CreateDocRequest : BaseRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Contents { get; set; }
        public List<int> TagIds { get; set; }
    }
}