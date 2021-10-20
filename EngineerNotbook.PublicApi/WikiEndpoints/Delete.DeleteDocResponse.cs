using System;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.WikiEndpoints
{
    /// <summary>
    /// Response for deleting a record of <see cref="Documentation"/> by ID
    /// </summary>
    public class DeleteDocResponse : BaseResponse
    {
        public DeleteDocResponse(Guid id) : base(id)
        {
            
        }

        public DeleteDocResponse()
        {
            
        }

        public string Status { get; set; } = "Deleted";
    }
}