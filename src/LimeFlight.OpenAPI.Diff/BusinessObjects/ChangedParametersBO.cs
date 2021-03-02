using System.Collections.Generic;
using System.Linq;
using LimeFlight.OpenAPI.Diff.Enums;
using LimeFlight.OpenAPI.Diff.Extensions;
using Microsoft.OpenApi.Models;

namespace LimeFlight.OpenAPI.Diff.BusinessObjects
{
    public class ChangedParametersBO : ComposedChangedBO
    {
        private readonly DiffContextBO _context;
        private readonly List<OpenApiParameter> _newParameterList;

        private readonly List<OpenApiParameter> _oldParameterList;

        public ChangedParametersBO(List<OpenApiParameter> oldParameterList, List<OpenApiParameter> newParameterList,
            DiffContextBO context)
        {
            _oldParameterList = oldParameterList;
            _newParameterList = newParameterList;
            _context = context;
            Increased = new List<OpenApiParameter>();
            Missing = new List<OpenApiParameter>();
            Changed = new List<ChangedParameterBO>();
        }

        public List<OpenApiParameter> Increased { get; set; }
        public List<OpenApiParameter> Missing { get; set; }
        public List<ChangedParameterBO> Changed { get; set; }
        protected override ChangedElementTypeEnum GetElementType() => ChangedElementTypeEnum.Parameter;

        public override List<(string Identifier, ChangedBO Change)> GetChangedElements()
        {
            return new List<(string Identifier, ChangedBO Change)>(
                    Changed.Select(x => (x.Name, (ChangedBO) x))
                )
                .Where(x => x.Change != null).ToList();
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (Increased.IsNullOrEmpty() && Missing.IsNullOrEmpty()) return new DiffResultBO(DiffResultEnum.NoChanges);

            if (!Increased.Any(x => x.Required) && Missing.IsNullOrEmpty())
                return new DiffResultBO(DiffResultEnum.Compatible);

            return new DiffResultBO(DiffResultEnum.Incompatible);
        }

        protected override List<ChangedInfoBO> GetCoreChanges() =>
            GetCoreChangeInfosOfComposed(Increased, Missing, x => x.Name);
    }
}