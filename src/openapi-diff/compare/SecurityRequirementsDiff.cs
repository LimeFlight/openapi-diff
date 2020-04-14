using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;
using openapi_diff.utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using openapi_diff.BusinessObjects;

namespace openapi_diff.compare
{
    public class SecurityRequirementsDiff
    {
        private OpenApiDiff openApiDiff;
        private OpenApiComponents leftComponents;
        private OpenApiComponents rightComponents;
        private static RefPointer<OpenApiSecurityScheme> refPointer = new RefPointer<OpenApiSecurityScheme>(RefTypeEnum.SecuritySchemes);

        public SecurityRequirementsDiff(OpenApiDiff openApiDiff)
        {
            this.openApiDiff = openApiDiff;
            leftComponents = openApiDiff.OldSpecOpenApi?.Components;
            rightComponents = openApiDiff.NewSpecOpenApi?.Components;
        }
        public OpenApiSecurityRequirement contains(List<OpenApiSecurityRequirement> securityRequirements, OpenApiSecurityRequirement left)
        {
            return securityRequirements
                .FirstOrDefault(x => Same(left, x));
        }

        public bool Same(OpenApiSecurityRequirement left, OpenApiSecurityRequirement right)
        {
            var leftTypes = GetListOfSecuritySchemes(leftComponents, left);
            var rightTypes = GetListOfSecuritySchemes(rightComponents, right);

            return leftTypes.SequenceEqual(rightTypes);
        }

        private static ImmutableDictionary<SecuritySchemeType, ParameterLocation> GetListOfSecuritySchemes(
            OpenApiComponents components, OpenApiSecurityRequirement securityRequirement)
        {
            var tmpResult = new Dictionary<SecuritySchemeType, ParameterLocation>();
            foreach (var openApiSecurityScheme in securityRequirement.Keys.ToList())
            {

                if (components.SecuritySchemes.TryGetValue(openApiSecurityScheme.Scheme, out var result))
                {
                    if (!tmpResult.ContainsKey(result.Type))
                        tmpResult.Add(result.Type, result.In);
                }
                else
                {
                    throw new ArgumentException("Impossible to find security scheme: " + openApiSecurityScheme.Scheme);
                }
            }
            return tmpResult.ToImmutableDictionary();
        }

        protected ChangedSecurityRequirementsBO Diff(
            List<OpenApiSecurityRequirement> left, List<OpenApiSecurityRequirement> right, DiffContextBO context)
        {
            left ??= new List<OpenApiSecurityRequirement>();
            var newRight = new OpenApiSecurityRequirement[] { };
            right?.CopyTo(newRight);
            right = newRight.ToList();

            var changedSecurityRequirements = new ChangedSecurityRequirementsBO(left, right);

            foreach (var leftSecurity in left)
            {
                var rightSecOpt = contains(right, leftSecurity);
                if (rightSecOpt != null)
                {
                    changedSecurityRequirements.Missing.Add(leftSecurity);
                }
                else
                {
                    var rightSec = rightSecOpt;

                    right.Remove(rightSec);
                    var diff =
                        openApiDiff.
                            SecurityRequirementDiff
                            .diff(leftSecurity, rightSec, context);
                    diff.ifPresent(changedSecurityRequirements::addChanged);
                }
            }

            right.forEach(changedSecurityRequirements::addIncreased);

            return isChanged(changedSecurityRequirements);
        }

        private List<OpenApiSecurityRequirement> GetCopy(List<OpenApiSecurityRequirement> right)
        {
            return right.Select(x => SecurityRequirementDiff.GetCopy(x))
            return right.stream().map(securitrequi).collect(Collectors.toList());
        }
    }
}
