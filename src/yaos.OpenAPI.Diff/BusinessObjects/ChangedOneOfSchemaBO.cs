using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using yaos.OpenAPI.Diff.Enums;
using yaos.OpenAPI.Diff.Extensions;

namespace yaos.OpenAPI.Diff.BusinessObjects
{
    public class ChangedOneOfSchemaBO : ComposedChangedBO
    {
        private readonly Dictionary<string, string> _oldMapping;
        private readonly Dictionary<string, string> _newMapping;
        private readonly DiffContextBO _context;

        public Dictionary<string, OpenApiSchema> Increased { get; set; }
        public Dictionary<string, OpenApiSchema> Missing { get; set; }
        public Dictionary<string, ChangedSchemaBO> Changed { get; set; }

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
