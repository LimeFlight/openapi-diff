﻿using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.DTOs;
using openapi_diff.utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace openapi_diff.compare
{
    public class SecurityRequirementsDiff
    {
        private readonly OpenApiDiff _openApiDiff;
        private readonly OpenApiComponents _leftComponents;
        private readonly OpenApiComponents _rightComponents;
        private static RefPointer<OpenApiSecurityScheme> _refPointer = new RefPointer<OpenApiSecurityScheme>(RefTypeEnum.SecuritySchemes);

        public SecurityRequirementsDiff(OpenApiDiff openApiDiff)
        {
            _openApiDiff = openApiDiff;
            _leftComponents = openApiDiff.OldSpecOpenApi?.Components;
            _rightComponents = openApiDiff.NewSpecOpenApi?.Components;
        }
        public OpenApiSecurityRequirement Contains(List<OpenApiSecurityRequirement> securityRequirements, OpenApiSecurityRequirement left)
        {
            return securityRequirements
                .FirstOrDefault(x => Same(left, x));
        }

        public bool Same(OpenApiSecurityRequirement left, OpenApiSecurityRequirement right)
        {
            var leftTypes = GetListOfSecuritySchemes(_leftComponents, left);
            var rightTypes = GetListOfSecuritySchemes(_rightComponents, right);

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
            right = right != null ? GetCopy(right) : new List<OpenApiSecurityRequirement>();

            var changedSecurityRequirements = new ChangedSecurityRequirementsBO(left, right);

            foreach (var leftSecurity in left)
            {
                var rightSecOpt = Contains(right, leftSecurity);
                if (rightSecOpt == null)
                {
                    changedSecurityRequirements.Missing.Add(leftSecurity);
                }
                else
                {
                    var rightSec = rightSecOpt;

                    right.Remove(rightSec);
                    var diff =
                        _openApiDiff.
                            SecurityRequirementDiff
                            .Diff(leftSecurity, rightSec, context);
                    if (diff != null)
                        changedSecurityRequirements.Changed.Add(diff);
                }
            }

            changedSecurityRequirements.Increased.AddRange(right);

            return ChangedUtils.IsChanged(changedSecurityRequirements);
        }

        private static List<OpenApiSecurityRequirement> GetCopy(IEnumerable<OpenApiSecurityRequirement> right)
        {
            return right.Select(SecurityRequirementDiff.GetCopy).ToList();
        }
    }
}
