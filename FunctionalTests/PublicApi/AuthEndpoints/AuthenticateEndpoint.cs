using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EngineerNotebook.Core.Constants;
using EngineerNotebook.Core.Extensions;
using EngineerNotebook.PublicApi.AuthEndpoints;
using Xunit;

namespace FunctionalTests.PublicApi.AuthEndpoints
{
    [Collection("Sequential")]
    public class AuthenticateEndpoint : IClassFixture<ApiTestFixture>
    {
        private JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public HttpClient Client { get; }

        public AuthenticateEndpoint(ApiTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        [Theory]
        [InlineData("admin@ddc.org", AuthorizationConstants.DEFAULT_PASSWORD, true)]
        [InlineData("admin@ddc.org", "badpassword", false)]
        public async Task ReturnsExpectedResultGivenCredentials(string username, string password, bool expectedResult)
        {
            var request = new AuthenticateRequest()
            {
                Username = username,
                Password = password
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("api/authenticate", jsonContent);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = stringResponse.FromJson<AuthenticateResponse>();

            Assert.Equal(expectedResult, model.Result);
        }
    }
}