using AspNetCore.Identity.Mongo.Model;

namespace EngineerNotebook.Shared.Models;
public class ClubMember : MongoUser<string>
{
    public string DisplayName { get; set; }
    public string GithubProfile { get; set; }
}
