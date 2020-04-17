using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using yaos.OpenAPI.Diff.Enums;
using yaos.OpenAPI.Diff.Extensions;

namespace yaos.OpenAPI.Diff.BusinessObjects
{
    public class ChangedHeadersBO : ComposedChangedBO
    {
        private readonly IDictionary<string, OpenApiHeader> _oldHeaders;
        private readonly IDictionary<string, OpenApiHeader> _newHeaders;
        private readonly DiffContextBO _context;

        public Dictionary<string, OpenApiHeader> Increased { get; set; }
        public Dictionary<string, OpenApiHeader> Missing { get; set; }
        public Dictionary<string, ChangedHeaderBO> Changed { get; set; }

        public ChangedHeadersBO(IDictionary<string, OpenApiHeader> oldHeaders, IDictionary<string, OpenApiHeader> newHeaders, DiffContextBO context)
        {
            _oldHeaders = oldHeaders;
            _newHeaders = newHeaders;
            _context = context;
        }


        public override List<ChangedBO> GetChangedElements()
        {
            return new List<ChangedBO>(Changed.Values);
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (Increased.IsNullOrEmpty() && Missing.IsNullOrEmpty())
            {
                return new DiffResultBO(DiffResultEnum.NoChanges);
            }
            if (Missing.IsNullOrEmpty())
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}
