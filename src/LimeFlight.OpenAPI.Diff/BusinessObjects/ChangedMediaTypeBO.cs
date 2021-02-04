using System.Collections.Generic;
using System.Linq;
using LimeFlight.OpenAPI.Diff.Enums;
using Microsoft.OpenApi.Models;

namespace LimeFlight.OpenAPI.Diff.BusinessObjects
{
    public class ChangedMediaTypeBO : ComposedChangedBO
    {
        private readonly DiffContextBO _context;
        private readonly OpenApiSchema _newSchema;

        private readonly OpenApiSchema _oldSchema;

        public ChangedMediaTypeBO(OpenApiSchema oldSchema, OpenApiSchema newSchema, DiffContextBO context)
        {
            _oldSchema = oldSchema;
            _newSchema = newSchema;
            _context = context;
        }

        public ChangedSchemaBO Schema { get; set; }

        protected override ChangedElementTypeEnum GetElementType()
        {
            return ChangedElementTypeEnum.MediaType;
        }

        public override List<(string Identifier, ChangedBO Change)> GetChangedElements()
        {
            return new List<(string Identifier, ChangedBO Change)>
                {
                    (null, Schema)
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