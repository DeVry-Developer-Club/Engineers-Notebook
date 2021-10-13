using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EngineersNotebook.Data.Models;

namespace EngineersNotebook.Models.Documentation
{
    public class CreateDocumentation
    {
        [Required]
        [DisplayName]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public string Contents { get; set; }

        public Tag[] Tags { get; set; }
    }
}