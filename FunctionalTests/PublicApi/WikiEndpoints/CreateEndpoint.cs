using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EngineerNotebook.Core.Extensions;
using EngineerNotebook.PublicApi.WikiEndpoints;
using Xunit;

namespace FunctionalTests.PublicApi.WikiEndpoints
{
    [Collection("Sequential")]
    public class CreateEndpoint : IClassFixture<ApiTestFixture>
    {
        private JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        
        private readonly int _testWikiId = 3;
        private readonly string _testWikiTitle = "I am a test";
        private readonly string _testWikiDescription = "I am a description";
        private readonly string _testWikiContent = "I am some awesome content";
        
        public HttpClient Client { get; }

        public CreateEndpoint(ApiTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        [Fact]
        public async Task ReturnsNotAuthorizedGivenNoUser()
        {
            var jsonContent = GetValidDocItem();
            var token = ApiTokenHelper.GetAdminUserToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.PostAsync("api/wiki", jsonContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ReturnsSuccessGivenValidDocAndAdminToken()
        {
            var jsonContent = GetValidDocItem();
            var adminToken = ApiTokenHelper.GetAdminUserToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            var response = await Client.PostAsync("api/wiki", jsonContent);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = stringResponse.FromJson<CreateDocResponse>();

            Assert.Equal(_testWikiId, model.Doc.Id);
            Assert.Equal(_testWikiTitle, model.Doc.Title);
            Assert.Equal(_testWikiDescription, model.Doc.Description);
            Assert.Equal(_testWikiContent, model.Doc.Contents);
            Assert.Equal("admin@ddc.org", model.Doc.CreatedByUserId);
        }
        
        private StringContent GetValidDocItem()
        {
            var request = new CreateDocRequest()
            {
                Title = _testWikiTitle,
                Description = _testWikiDescription,
                Contents = _testWikiContent
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            return jsonContent;
        }
    }
}