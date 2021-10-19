using System.Threading.Tasks;

namespace EngineerNotebook.Core.Interfaces
{
    /// <summary>
    /// Service which converts a Razor or CSHTML Page into an HTML string
    /// </summary>
    public interface IRazorToString
    {
        /// <summary>
        /// Compile / Render a CSHTML view.
        /// </summary>
        /// <param name="view">CSHTML view to render</param>
        /// <param name="model">Data to pass into <paramref name="view"/></param>
        /// <returns>The compiled HTML output of view</returns>
        Task<string> ToViewString(string view, object model);
    }
}