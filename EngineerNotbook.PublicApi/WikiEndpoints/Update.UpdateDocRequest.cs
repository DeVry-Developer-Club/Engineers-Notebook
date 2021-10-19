using System.ComponentModel.DataAnnotations;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    /// <summary>
    /// Request for updating <see cref="Documentation"/>
    /// </summary>
    public class UpdateDocRequest : BaseRequest
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public string Contents { get; set; }

        public int[] TagIds { get; set; }
    }
}