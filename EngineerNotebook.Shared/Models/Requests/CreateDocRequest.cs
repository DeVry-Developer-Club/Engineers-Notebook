using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EngineerNotebook.Shared.Models.Requests
{
    public class CreateDocRequest
    {
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string Contents { get; set; }
        public List<int> TagIds { get; set; }
    }
}