using AspNetCore.Identity.MongoDbCore.Models;

namespace EngineerNotebook.Shared.Models;
public class ClubMember : MongoIdentityUser<System.Guid>
{
    public string DisplayName { get; set; }
    public string GithubProfile { get; set; }
}
