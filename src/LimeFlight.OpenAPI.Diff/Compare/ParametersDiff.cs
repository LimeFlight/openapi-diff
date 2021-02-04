using System.Collections.Generic;
using System.Linq;
using LimeFlight.OpenAPI.Diff.BusinessObjects;
using LimeFlight.OpenAPI.Diff.Enums;
using LimeFlight.OpenAPI.Diff.Utils;
using Microsoft.OpenApi.Models;

namespace LimeFlight.OpenAPI.Diff.Compare
{
    public class ParametersDiff
    {
        private static readonly RefPointer<OpenApiParameter> RefPointer =
            new RefPointer<OpenApiParameter>(RefTypeEnum.Parameters);

        private readonly OpenApiComponents _leftComponents;
        private readonly OpenApiDiff _openApiDiff;
        private readonly OpenApiComponents _rightComponents;

        public ParametersDiff(OpenApiDiff openApiDiff)
        {
            _openApiDiff = openApiDiff;
            _leftComponents = openApiDiff.OldSpecOpenApi?.Components;
            _rightComponents = openApiDiff.NewSpecOpenApi?.Components;
        }

        public static OpenApiParameter Contains(OpenApiComponents components, List<OpenApiParameter> parameters,
            OpenApiParameter parameter)
        {
            return parameters
                .FirstOrDefault(x =>
                    Same(RefPointer.ResolveRef(components, x, x.Reference?.ReferenceV3), parameter));
        }

        public static bool Same(OpenApiParameter left, OpenApiParameter right)
        {
            return left.Name == right.Name && left.In.Equals(right.In);
        }

        public ChangedParametersBO Diff(
            List<OpenApiParameter> left, List<OpenApiParameter> right, DiffContextBO context)
        {
            var changedParameters =
                new ChangedParametersBO(left, right, context);
            if (null == left) left = new List<OpenApiParameter>();
            if (null == right) right = new List<OpenApiParameter>();

            foreach (var openApiParameter in left)
            {
                var leftPara = openApiParameter;
                leftPara = RefPointer.ResolveRef(_leftComponents, leftPara, leftPara.Reference?.ReferenceV3);

                var rightParam = Contains(_rightComponents, right, leftPara);
                if (rightParam == null)
                {
                    changedParameters.Missing.Add(leftPara);
                }
                else
                {
                    right.Remove(rightParam);

                    var diff = _openApiDiff.ParameterDiff
                        .Diff(leftPara, rightParam, context);
                    if (diff != null)
                        changedParameters.Changed.Add(diff);
                }
            }

            changedParameters.Increased.AddRange(right);

            return ChangedUtils.IsChanged(changedParameters);
        }
    }
}