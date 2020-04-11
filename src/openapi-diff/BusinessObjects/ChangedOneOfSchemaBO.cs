using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;
using openapi_diff.Extensions;
using System.Collections.Generic;

namespace openapi_diff.BusinessObjects
{
    public class ChangedOneOfSchemaBO : ComposedChangedBO
    {
        private readonly Dictionary<string, string> _oldMapping;
        private readonly Dictionary<string, string> _newMapping;
        private readonly DiffContextBO _context;

        private Dictionary<string, OpenApiSchema> Increased { get; set; }
        private Dictionary<string, OpenApiSchema> Missing { get; set; }
        private Dictionary<string, ChangedSchemaBO> Changed { get; set; }

        public ChangedOneOfSchemaBO(
            Dictionary<string, string> oldMapping,
            Dictionary<string, string> newMapping,
            DiffContextBO context)
        {
            _oldMapping = oldMapping;
            _newMapping = newMapping;
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
            if (_context.IsRequest && Missing.IsNullOrEmpty() || _context.IsResponse && Increased.IsNullOrEmpty())
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}
