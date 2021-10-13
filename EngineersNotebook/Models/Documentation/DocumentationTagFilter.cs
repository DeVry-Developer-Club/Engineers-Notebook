using System.Text;

namespace EngineersNotebook.Models.Documentation
{
    public class DocumentationTagFilter
    {
        public string[] TagNames { get; set; }
        public int[] TagIds { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new();

            if (TagNames is not null)
                builder.AppendLine(string.Join("\t", TagNames));

            if (TagIds is not null)
                builder.AppendLine(string.Join("\t", TagIds));

            return builder.ToString();
        }
    }
}