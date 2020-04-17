using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using yaos.OpenAPI.Diff.Enums;

namespace yaos.OpenAPI.Diff.BusinessObjects
{
    public class ChangedMediaTypeBO : ComposedChangedBO
    {
        private readonly OpenApiSchema _oldSchema;
        private readonly OpenApiSchema _newSchema;
        private readonly DiffContextBO _context;

        public ChangedSchemaBO Schema { get; set; }

        public ChangedMediaTypeBO(OpenApiSchema oldSchema, OpenApiSchema newSchema, DiffContextBO context)
        {
            _oldSchema = oldSchema;
            _newSchema = newSchema;
            _context = context;
        }

        public override List<ChangedBO> GetChangedElements()
        {
            return new List<ChangedBO> { Schema };
        }

        public override DiffResultBO IsCoreChanged()
        {
            return new DiffResultBO(DiffResultEnum.NoChanges);
        }
    }
}
