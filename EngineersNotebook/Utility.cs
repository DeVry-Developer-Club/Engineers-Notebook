using System.IO;
using System.Threading.Tasks;
using Razor.Templating.Core;
using SelectPdf;

namespace EngineersNotebook
{
    /// <summary>
    /// Provides helper functions for the project
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Convert HTML string into a PDF
        /// </summary>
        /// <param name="html"></param>
        /// <returns>PDF in the form of byte array</returns>
        public static async Task<byte[]> HtmlToPdf(string html)
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.MarginBottom = 10;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            
            PdfDocument doc = converter.ConvertHtmlString(html);
            
            using MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            return stream.ToArray();
        }
        
        /// <summary>
        /// Compile / Render a CSHTML view. 
        /// </summary>
        /// <param name="view">CSHTML view to render</param>
        /// <param name="model">Data to pass into <paramref name="view"/></param>
        /// <returns>String representation of view</returns>
        public static async Task<string> ToViewString(string view, object model) => await RazorTemplateEngine.RenderAsync(view, model);
    }
}