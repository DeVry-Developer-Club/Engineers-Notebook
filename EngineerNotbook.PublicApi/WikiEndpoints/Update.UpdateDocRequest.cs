using System.ComponentModel.DataAnnotations;

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    public class UpdateDocRequest : BaseRequest
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public string Contents { get; set; }
    }
}