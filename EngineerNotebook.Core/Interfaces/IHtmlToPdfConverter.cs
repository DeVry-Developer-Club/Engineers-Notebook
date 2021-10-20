using System.Threading.Tasks;

namespace EngineerNotebook.Core.Interfaces
{
    /// <summary>
    /// Service which converts HTML into a PDF
    /// </summary>
    public interface IHtmlToPdfConverter
    {
        /// <summary>
        /// Convert HTML string into a PDF
        /// </summary>
        /// <param name="html"></param>
        /// <returns>PDF in the form of a byte array</returns>
        Task<byte[]> HtmlToPdf(string html);
    }
}