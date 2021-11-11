using System.IO;
using System.Threading.Tasks;
using EngineerNotebook.Core.Interfaces;
using SelectPdf;

namespace EngineerNotebook.Infrastructure.Utility
{
    public class HtmlToPdfConverter : IHtmlToPdfConverter
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<byte[]> HtmlToPdf(string html)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
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
    }
}