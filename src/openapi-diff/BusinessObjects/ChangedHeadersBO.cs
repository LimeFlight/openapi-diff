using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;
using openapi_diff.Extensions;
using System.Collections.Generic;

namespace openapi_diff.BusinessObjects
{
    public class ChangedHeadersBO : ComposedChangedBO
    {
        private readonly Dictionary<string, OpenApiHeader> _oldHeaders;
        private readonly Dictionary<string, OpenApiHeader> _newHeaders;
        private readonly DiffContextBO _context;

        public Dictionary<string, OpenApiHeader> Increased { get; set; }
        public Dictionary<string, OpenApiHeader> Missing { get; set; }
        public Dictionary<string, ChangedHeaderBO> Changed { get; set; }

        public ChangedHeadersBO(Dictionary<string, OpenApiHeader> oldHeaders, Dictionary<string, OpenApiHeader> newHeaders, DiffContextBO context)
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
