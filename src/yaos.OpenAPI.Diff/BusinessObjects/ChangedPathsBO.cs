using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using yaos.OpenAPI.Diff.Enums;
using yaos.OpenAPI.Diff.Extensions;

namespace yaos.OpenAPI.Diff.BusinessObjects
{
    public class ChangedPathsBO : ComposedChangedBO
    {
        private readonly Dictionary<string, OpenApiPathItem> _oldPathMap;
        private readonly Dictionary<string, OpenApiPathItem> _newPathMap;

        public Dictionary<string, OpenApiPathItem> Increased { get; set; }
        public Dictionary<string, OpenApiPathItem> Missing { get; set; }
        public Dictionary<string, ChangedPathBO> Changed { get; set; }

        public ChangedPathsBO(Dictionary<string, OpenApiPathItem> oldPathMap, Dictionary<string, OpenApiPathItem> newPathMap)
        {
            _oldPathMap = oldPathMap;
            _newPathMap = newPathMap;
            Increased = new Dictionary<string, OpenApiPathItem>();
            Missing = new Dictionary<string, OpenApiPathItem>();
            Changed = new Dictionary<string, ChangedPathBO>();
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
