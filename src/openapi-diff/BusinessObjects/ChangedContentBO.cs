using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;
using openapi_diff.Extensions;
using System.Collections.Generic;

namespace openapi_diff.BusinessObjects
{
    public class ChangedContentBO : ComposedChangedBO
    {
        private readonly Content _oldContent;
        private readonly Content _newContent;
        private readonly DiffContextBO _context;

        private Dictionary<string, OpenApiMediaType> Increased { get; set; }
        private Dictionary<string, OpenApiMediaType> Missing { get; set; }
        public Dictionary<string, ChangedMediaTypeBO> Changed { get; set; }

        public ChangedContentBO(Content oldContent, Content newContent, DiffContextBO context)
        {
            _oldContent = oldContent;
            _newContent = newContent;
            _context = context;
            Increased = new Dictionary<string, OpenApiMediaType>();
            Missing = new Dictionary<string, OpenApiMediaType>();
            Changed = new Dictionary<string, ChangedMediaTypeBO>();
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
