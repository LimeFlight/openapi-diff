using System.Collections.Generic;
using System.Linq;
using LimeFlight.OpenAPI.Diff.Enums;
using LimeFlight.OpenAPI.Diff.Extensions;
using Microsoft.OpenApi.Models;

namespace LimeFlight.OpenAPI.Diff.BusinessObjects
{
    public class ChangedOneOfSchemaBO : ComposedChangedBO
    {
        private readonly Dictionary<string, string> _newMapping;

        private readonly Dictionary<string, string> _oldMapping;

        public ChangedOneOfSchemaBO(Dictionary<string, string> oldMapping, Dictionary<string, string> newMapping,
            DiffContextBO context)
        {
            _oldMapping = oldMapping;
            _newMapping = newMapping;
            Context = context;
        }

        public DiffContextBO Context { get; }

        public Dictionary<string, OpenApiSchema> Increased { get; set; }
        public Dictionary<string, OpenApiSchema> Missing { get; set; }
        public Dictionary<string, ChangedSchemaBO> Changed { get; set; }

        protected override ChangedElementTypeEnum GetElementType()
        {
            return ChangedElementTypeEnum.OneOf;
        }

        public override List<(string Identifier, ChangedBO Change)> GetChangedElements()
        {
            return new List<(string Identifier, ChangedBO Change)>(
                    Changed.Select(x => (x.Key, (ChangedBO) x.Value))
                )
                .Where(x => x.Change != null).ToList();
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (Increased.IsNullOrEmpty() && Missing.IsNullOrEmpty()) return new DiffResultBO(DiffResultEnum.NoChanges);
            if (Context.IsRequest && Missing.IsNullOrEmpty() || Context.IsResponse && Increased.IsNullOrEmpty())
                return new DiffResultBO(DiffResultEnum.Compatible);
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }

        protected override List<ChangedInfoBO> GetCoreChanges()
        {
            return GetCoreChangeInfosOfComposed(Increased.Keys.ToList(), Missing.Keys.ToList(), x => x);
        }
    }
}