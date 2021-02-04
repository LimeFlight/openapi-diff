using System.Collections.Generic;
using System.Linq;
using LimeFlight.OpenAPI.Diff.Enums;
using Microsoft.OpenApi.Models;

namespace LimeFlight.OpenAPI.Diff.BusinessObjects
{
    public class ChangedResponseBO : ComposedChangedBO
    {
        private readonly DiffContextBO _context;

        public ChangedResponseBO(OpenApiResponse oldApiResponse, OpenApiResponse newApiResponse, DiffContextBO context)
        {
            OldApiResponse = oldApiResponse;
            NewApiResponse = newApiResponse;
            _context = context;
        }

        public OpenApiResponse OldApiResponse { get; }
        public OpenApiResponse NewApiResponse { get; }
        public ChangedMetadataBO Description { get; set; }
        public ChangedHeadersBO Headers { get; set; }
        public ChangedContentBO Content { get; set; }
        public ChangedExtensionsBO Extensions { get; set; }
        protected override ChangedElementTypeEnum GetElementType() => ChangedElementTypeEnum.Response;

        public override List<(string Identifier, ChangedBO Change)> GetChangedElements()
        {
            return new List<(string Identifier, ChangedBO Change)>
                {
                    (null, Description),
                    (null, Headers),
                    (null, Content),
                    (null, Extensions)
                }
                .Where(x => x.Change != null).ToList();
        }

        public override DiffResultBO IsCoreChanged()
        {
            return new DiffResultBO(DiffResultEnum.NoChanges);
        }

        protected override List<ChangedInfoBO> GetCoreChanges()
        {
            return new List<ChangedInfoBO>();
        }
    }
}