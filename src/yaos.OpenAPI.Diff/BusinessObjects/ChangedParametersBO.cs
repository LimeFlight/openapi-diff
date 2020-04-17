using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using yaos.OpenAPI.Diff.Enums;
using yaos.OpenAPI.Diff.Extensions;

namespace yaos.OpenAPI.Diff.BusinessObjects
{
    public class ChangedParametersBO : ComposedChangedBO
    {
        private readonly List<OpenApiParameter> _oldParameterList;
        private readonly List<OpenApiParameter> _newParameterList;
        private readonly DiffContextBO _context;
        public List<OpenApiParameter> Increased { get; set; }
        public List<OpenApiParameter> Missing { get; set; }
        public List<ChangedParameterBO> Changed { get; set; }

        public ChangedParametersBO(List<OpenApiParameter> oldParameterList, List<OpenApiParameter> newParameterList, DiffContextBO context)
        {
            _oldParameterList = oldParameterList;
            _newParameterList = newParameterList;
            _context = context;
            Increased = new List<OpenApiParameter>();
            Missing = new List<OpenApiParameter>();
            Changed = new List<ChangedParameterBO>();
        }

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
