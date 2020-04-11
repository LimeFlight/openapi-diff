using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;
using openapi_diff.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace openapi_diff.BusinessObjects
{
    public class ChangedParametersBO : ComposedChangedBO
    {
        public List<OpenApiParameter> OldParameterList { get; set; }
        public List<OpenApiParameter> NewParameterList { get; set; }
        public DiffContextDTO Context { get; set; }
        public List<OpenApiParameter> Increased { get; set; }
        public List<OpenApiParameter> Missing { get; set; }
        public List<ChangedParametersBO> Changed { get; set; }

        public override List<ChangedBO> GetChangedElements()
        {
            return new List<ChangedBO>(Changed);
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (Increased.IsNullOrEmpty() && Missing.IsNullOrEmpty())
            {
                return new DiffResultBO(DiffResultEnum.NoChanges);
            }

            if (Increased.Any(x => x.Required) && Missing.IsNullOrEmpty())
                return new DiffResultBO(DiffResultEnum.Compatible);

            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}
