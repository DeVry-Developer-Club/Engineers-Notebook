using System.IO;
using System.Threading.Tasks;
using EngineerNotebook.Core.Interfaces;
using SelectPdf;

namespace EngineerNotebook.Infrastructure.Utility
{
    public class HtmlToPdfConverter : IHtmlToPdfConverter
    {
        public async Task<byte[]> HtmlToPdf(string html)
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