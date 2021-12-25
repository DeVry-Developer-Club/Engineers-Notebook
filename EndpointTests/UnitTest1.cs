using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Grpc.Core;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text.RegularExpressions;

namespace EndpointTests;
[TestClass]
public class UnitTest1
{
    const string RemoteAddress = "https://localhost:5099";
        
    [TestMethod]
    public void Test()
    {
        Regex regex = new(@"^\d+$");

        string test = "123456";

        Assert.IsTrue(regex.IsMatch(test));
        Assert.IsFalse(regex.IsMatch("1234asdf"));
    }
        
    [TestMethod]
    public async Task AddTagToItem()
    {
        //var authClient = new Authentication.AuthenticationClient(GrpcChannel.ForAddress(RemoteAddress));
        //var client = new Tags.TagsClient(GrpcChannel.ForAddress(RemoteAddress));
        //var docClient = new EngineerNotebook.GrpcContracts.Documents.Documents.DocumentsClient(GrpcChannel.ForAddress(RemoteAddress));

        //var user = await authClient.AuthenticateAsync(new()
        //{
        //    Username = "admin@ddc.org",
        //    Password = "Pass@word1"
        //});

        //Assert.IsTrue(user.Result);
        //Assert.IsNotNull(user.Token);

        //var headerMetadata = new Metadata();
        //headerMetadata.Add("Authorization", $"Bearer {user.Token}");
        //CallOptions callOptions = new CallOptions(headerMetadata);

        //var existingDocs = docClient.GetList(new(), callOptions);
        //var existingTags = client.GetList(new(), callOptions);

        //List<TagDto> tags = new();
        //List<EngineerNotebook.GrpcContracts.Documents.DocDto> docs = new();

        //await foreach(var tag in existingTags.ResponseStream.AsAsyncEnumerableAsync())
        //    tags.Add(tag);

        //await foreach (var doc in existingDocs.ResponseStream.AsAsyncEnumerableAsync())
        //    docs.Add(doc);

        //Assert.IsTrue(tags.Count > 0);
        //Assert.IsTrue(docs.Count > 0);

        //var firstDoc = docs.First();
        //var tagsToAdd = tags.Take(3);

        //EngineerNotebook.GrpcContracts.Documents.UpdateRequest request = new EngineerNotebook.GrpcContracts.Documents.UpdateRequest()
        //{
        //    Contents = firstDoc.Contents,
        //    Id = firstDoc.Id,
        //    Description = firstDoc.Description,
        //    Title = firstDoc.Title
        //};

        //request.TagIds.AddRange(tagsToAdd.Select(x => x.Id));

        //var response = await docClient.UpdateAsync(request, callOptions);

        //Assert.IsNotNull(response);
    }

}
