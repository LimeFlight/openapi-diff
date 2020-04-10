using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OpenApi.Models;

namespace openapi_diff.DTOs
{
    public class ChangedRequestBodyDTO
    {
        private OpenApiRequestBody oldRequestBody;
        private OpenApiRequestBody newRequestBody;
        private DiffContextDTO context;

        public bool changeRequired;
        public ChangedMetadataDTO description;
        public ChangedContentDTO content;
        public ChangedExtensionsDTO extensions;
    }
}
