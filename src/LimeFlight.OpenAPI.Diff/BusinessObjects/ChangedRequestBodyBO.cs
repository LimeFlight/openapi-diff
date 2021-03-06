﻿using System.Collections.Generic;
using System.Linq;
using LimeFlight.OpenAPI.Diff.Enums;
using Microsoft.OpenApi.Models;

namespace LimeFlight.OpenAPI.Diff.BusinessObjects
{
    public class ChangedRequestBodyBO : ComposedChangedBO
    {
        private readonly DiffContextBO _context;
        private readonly OpenApiRequestBody _newRequestBody;

        private readonly OpenApiRequestBody _oldRequestBody;

        public ChangedRequestBodyBO(OpenApiRequestBody oldRequestBody, OpenApiRequestBody newRequestBody,
            DiffContextBO context)
        {
            _oldRequestBody = oldRequestBody;
            _newRequestBody = newRequestBody;
            _context = context;
        }

        public bool ChangeRequired { get; set; }
        public ChangedMetadataBO Description { get; set; }
        public ChangedContentBO Content { get; set; }
        public ChangedExtensionsBO Extensions { get; set; }
        protected override ChangedElementTypeEnum GetElementType() => ChangedElementTypeEnum.RequestBody;

        public override List<(string Identifier, ChangedBO Change)> GetChangedElements()
        {
            return new List<(string Identifier, ChangedBO Change)>
                {
                    ("Description", Description),
                    ("Content", Content),
                    (null, Extensions)
                }
                .Where(x => x.Change != null).ToList();
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (!ChangeRequired) return new DiffResultBO(DiffResultEnum.NoChanges);
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }

        protected override List<ChangedInfoBO> GetCoreChanges()
        {
            var returnList = new List<ChangedInfoBO>();
            var elementType = GetElementType();
            const TypeEnum changeType = TypeEnum.Changed;

            if (_oldRequestBody?.Required != _newRequestBody?.Required)
                returnList.Add(new ChangedInfoBO(elementType, changeType, "Required",
                    _oldRequestBody?.Required.ToString(), _newRequestBody?.Required.ToString()));

            return returnList;
        }
    }
}