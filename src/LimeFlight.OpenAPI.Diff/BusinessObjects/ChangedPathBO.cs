using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;
using LimeFlight.OpenAPI.Diff.Enums;
using LimeFlight.OpenAPI.Diff.Extensions;

namespace LimeFlight.OpenAPI.Diff.BusinessObjects
{
    public class ChangedPathBO : ComposedChangedBO
    {
        protected override ChangedElementTypeEnum GetElementType() => ChangedElementTypeEnum.Path;

        private readonly string _pathUrl;
        private readonly OpenApiPathItem _oldPath;
        private readonly OpenApiPathItem _newPath;
        private readonly DiffContextBO _context;

        public Dictionary<OperationType, OpenApiOperation> Increased { get; set; }
        public Dictionary<OperationType, OpenApiOperation> Missing { get; set; }
        public List<ChangedOperationBO> Changed { get; set; }
        public ChangedExtensionsBO Extensions { get; set; }

        public ChangedPathBO(string pathUrl, OpenApiPathItem oldPath, OpenApiPathItem newPath, DiffContextBO context)
        {
            _pathUrl = pathUrl;
            _oldPath = oldPath;
            _newPath = newPath;
            _context = context;
            Increased = new Dictionary<OperationType, OpenApiOperation>();
            Missing = new Dictionary<OperationType, OpenApiOperation>();
            Changed = new List<ChangedOperationBO>();
        }

        public override List<(string Identifier, ChangedBO Change)> GetChangedElements()
        {
            return new List<(string Identifier, ChangedBO Change)>(
                    Changed.Select(x => (x.PathUrl, (ChangedBO)x))
                )
                {
                    (null, Extensions)
                }
                .Where(x => x.Change != null).ToList();
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

        protected override List<ChangedInfoBO> GetCoreChanges() =>
            GetCoreChangeInfosOfComposed(Increased.Keys.ToList(), Missing.Keys.ToList(), x => x.ToString());
    }
}
