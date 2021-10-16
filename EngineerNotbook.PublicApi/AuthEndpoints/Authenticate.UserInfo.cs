using System.Collections.Generic;

namespace EngineerNotebook.PublicApi.AuthEndpoints
{
    public class UserInfo
    {
        public static readonly UserInfo Anonymous = new();

        public bool IsAuthenticated { get; set; }
        public string NameClaimType { get; set; }
        public string RoleClaimType { get; set; }
        public IEnumerable<ClaimValue> Claims { get; set; }
    }
}