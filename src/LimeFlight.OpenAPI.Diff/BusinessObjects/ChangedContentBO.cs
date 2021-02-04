using System.Collections.Generic;
using System.Linq;
using LimeFlight.OpenAPI.Diff.Enums;
using LimeFlight.OpenAPI.Diff.Extensions;
using Microsoft.OpenApi.Models;

namespace LimeFlight.OpenAPI.Diff.BusinessObjects
{
    public class ChangedContentBO : ComposedChangedBO
    {
        private readonly DiffContextBO _context;
        private readonly Dictionary<string, OpenApiMediaType> _newContent;

        private readonly Dictionary<string, OpenApiMediaType> _oldContent;

        public ChangedContentBO(Dictionary<string, OpenApiMediaType> oldContent,
            Dictionary<string, OpenApiMediaType> newContent, DiffContextBO context)
        {
            _oldContent = oldContent;
            _newContent = newContent;
            _context = context;
            Increased = new Dictionary<string, OpenApiMediaType>();
            Missing = new Dictionary<string, OpenApiMediaType>();
            Changed = new Dictionary<string, ChangedMediaTypeBO>();
        }

        public Dictionary<string, OpenApiMediaType> Increased { get; set; }
        public Dictionary<string, OpenApiMediaType> Missing { get; set; }
        public Dictionary<string, ChangedMediaTypeBO> Changed { get; set; }

        protected override ChangedElementTypeEnum GetElementType()
        {
            return ChangedElementTypeEnum.Content;
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
            if (_context.IsRequest && Missing.IsNullOrEmpty() || _context.IsResponse && Increased.IsNullOrEmpty())
                return new DiffResultBO(DiffResultEnum.Compatible);
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }

        protected override List<ChangedInfoBO> GetCoreChanges()
        {
            return GetCoreChangeInfosOfComposed(Increased.Keys.ToList(), Missing.Keys.ToList(), x => x);
        }
    }
}