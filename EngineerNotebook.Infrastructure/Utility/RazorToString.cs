using System.Threading.Tasks;
using EngineerNotebook.Core.Interfaces;
using Razor.Templating.Core;

namespace EngineerNotebook.Infrastructure.Utility
{
    public class RazorToString : IRazorToString
    {
        public async Task<string> ToViewString(string view, object model)
            => await RazorTemplateEngine.RenderAsync(view, model);
    }
}