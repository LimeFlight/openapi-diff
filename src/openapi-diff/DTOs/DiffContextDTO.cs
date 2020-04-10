using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace openapi_diff.DTOs
{
    public class DiffContextDTO
    {
        public string URL { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public OperationType Method { get; set; }
        public bool Response { get; set; }
        public bool Request { get; set; }
        public bool Required { get; set; }
    }
}
