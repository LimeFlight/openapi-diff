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
        public DiffContextBO Context { get; }

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
            Context = context;
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
            if (Context.IsRequest && Missing.IsNullOrEmpty() || Context.IsResponse && Increased.IsNullOrEmpty())
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}
