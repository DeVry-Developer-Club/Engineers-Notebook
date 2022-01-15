using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EngineerNotebook.Shared.Models;

namespace EngineerNotebook.Core.Interfaces;

public interface IGuideService
{
    Task<byte[]> GetGuide(List<string> found, CancellationToken token = default);
    Task<byte[]> GetGuide(List<Tag> found, CancellationToken token = default);
}