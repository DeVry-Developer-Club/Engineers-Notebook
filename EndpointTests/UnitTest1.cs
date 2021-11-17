using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineerNotebook.GrpcContracts.Authentication;
using EngineerNotebook.GrpcContracts.Guide;
using Grpc.Net.Client;
using System.Threading.Tasks;
using Grpc.Core;
using System.Collections.Generic;
using System.Threading;
using EngineerNotebook.GrpcContracts.Tags;
using EngineerNotebook.GrpcContracts;
using System.Linq;

namespace EndpointTests;
[TestClass]
public class UnitTest1
{
    const string RemoteAddress = "https://localhost:5099";

    [TestMethod]
    public async Task TestLocalAuthentication()
    {
        var client = new Authentication.AuthenticationClient(GrpcChannel.ForAddress(RemoteAddress));

        var result = await client.AuthenticateAsync(new()
        {
            Username = "admin@ddc.org",
            Password = "Pass@word1"
        });

        Assert.IsTrue(result.Result);
        Assert.IsNotNull(result.Token);
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

    [TestMethod]
    public async Task TestLocalGuide()
    {
        var client = new Guides.GuidesClient(GrpcChannel.ForAddress(RemoteAddress));
        GetByTagsRequest request = new GetByTagsRequest();
        request.TagIds.AddRange(new[]
        {
            "61943caba67b5062d533ab91",
            "61943caba67b5062d533ab83",
            "61943caba67b5062d533ab85"
        });

        var response = await client.GuideAsync(request);
        await System.IO.File.WriteAllBytesAsync("test.pdf", response.Guide.ToByteArray());
        Assert.IsNotNull(response);
        Assert.IsNotNull(response.ContentType);
        Assert.IsNotNull(response.Guide);
    }
}
