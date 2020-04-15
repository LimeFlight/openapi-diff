using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;
using openapi_diff.Extensions;
using System.Collections.Generic;

namespace openapi_diff.BusinessObjects
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
            this._oldPathMap = oldPathMap;
            this._newPathMap = newPathMap;
            this.Increased = new Dictionary<string, OpenApiPathItem>();
            this.Missing = new Dictionary<string, OpenApiPathItem>();
            this.Changed = new Dictionary<string, ChangedPathBO>();
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
