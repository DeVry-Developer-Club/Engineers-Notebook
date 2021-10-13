using System.Collections.Generic;
using Newtonsoft.Json;

namespace EngineersNotebook.Data.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public List<Documentation> Docs { get; set; }
    }
}