using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.OpenApi.Models;

namespace openapi_diff.compare
{
    public class OpenApiDiff
    {
        private PathsDiff _pathsDiff;
        private PathDiff _pathDiff;
        private SchemaDiff _schemaDiff;


        public string Compare(OpenApiDocument oldSpec, OpenApiDocument newSpec)
        {
            var securityRequirements = oldSpec.SecurityRequirements;

            if (securityRequirements != null)
            {
                var distinctSecurityRequirements = securityRequirements.Distinct();
            }

        }

        private void InitializeFields()
        {
            _pathsDiff = new PathsDiff(this);
            _pathDiff = new PathDiff(this);
        }
    }
}
