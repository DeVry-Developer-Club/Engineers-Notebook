using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineerNotebook.GrpcContracts.Authentication;
using Grpc.Net.Client;
using System.Threading.Tasks;

namespace EndpointTests;
[TestClass]
public class UnitTest1
{
    const string RemoteAddress = "https://localhost:5099";

    [TestMethod]
    public async Task TestRemoteAuthentication()
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
}
