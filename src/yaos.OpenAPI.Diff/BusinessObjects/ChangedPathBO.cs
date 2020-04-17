using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using yaos.OpenAPI.Diff.Enums;
using yaos.OpenAPI.Diff.Extensions;

namespace yaos.OpenAPI.Diff.BusinessObjects
{
    public class ChangedPathBO : ComposedChangedBO
    {
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


        public override List<ChangedBO> GetChangedElements()
        {
            return new List<ChangedBO>(Changed) { Extensions };
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
