using System;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.PublicApi.TagEndpoints
{
    /// <summary>
    /// Response for deleting a record of <see cref="Tag"/> by ID
    /// </summary>
    public class DeleteTagResponse : BaseResponse
    {
        public DeleteTagResponse()
        {
        }

        public DeleteTagResponse(Guid correlationId) : base(correlationId)
        {
        }

        public string Status { get; set; } = "Deleted";
    }
}