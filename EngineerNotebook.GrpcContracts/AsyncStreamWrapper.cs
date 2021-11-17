using Grpc.Core;
using System.Runtime.CompilerServices;

namespace EngineerNotebook.GrpcContracts;
public static class AsyncStreamWrapper
{
    /// <summary>
    /// Allow the consumption of <paramref name="stream"/> as an IAsyncEnumerable (awaitable for-each)
    /// </summary>
    /// <typeparam name="TEntity">Type of data the stream is streaming in</typeparam>
    /// <param name="stream">Stream from GrpcServer to GrpcClient</param>
    /// <param name="cancellationToken">Cancellation token that will be used to dispose of resources</param>
    /// <returns>Yields an instance of <typeparamref name="TEntity"/> as it trickles in from <paramref name="stream"/></returns>
    public static async IAsyncEnumerable<TEntity> AsAsyncEnumerableAsync<TEntity>(this IAsyncStreamReader<TEntity> stream, [EnumeratorCancellation]CancellationToken cancellationToken = default)
    {
        bool next;

        do
        {
            next = await stream.MoveNext(cancellationToken);
            yield return stream.Current;
        } while (next);
    }
}
