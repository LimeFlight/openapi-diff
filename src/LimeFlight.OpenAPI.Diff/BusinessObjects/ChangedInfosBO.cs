using System.Collections.Generic;

namespace LimeFlight.OpenAPI.Diff.BusinessObjects
{
    public class ChangedInfosBO
    {
        public List<string> Path { get; set; }
        public DiffResultBO ChangeType { get; set; }
        public List<ChangedInfoBO> Changes { get; set; }
    }
}