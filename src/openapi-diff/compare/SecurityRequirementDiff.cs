using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.Extensions;
using openapi_diff.utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace openapi_diff.compare
{
    public class SecurityRequirementDiff
    {
        private readonly OpenApiDiff _openApiDiff;
        private readonly OpenApiComponents _leftComponents;
        private readonly OpenApiComponents _rightComponents;

        public SecurityRequirementDiff(OpenApiDiff openApiDiff)
        {
            _openApiDiff = openApiDiff;
            _leftComponents = openApiDiff.OldSpecOpenApi?.Components;
            _rightComponents = openApiDiff.NewSpecOpenApi?.Components;
        }
        public static OpenApiSecurityRequirement GetCopy(Dictionary<OpenApiSecurityScheme, IList<string>> right)
        {
            var newSecurityRequirement = new OpenApiSecurityRequirement();
            foreach (var (key, value) in right)
            {
                newSecurityRequirement.Add(key, value);
            }

            return newSecurityRequirement;
        }

        private OpenApiSecurityRequirement Contains(
            OpenApiSecurityRequirement right, string schemeRef)
        {
            var leftSecurityScheme = _leftComponents.SecuritySchemes[schemeRef];
            var found = new OpenApiSecurityRequirement();

            foreach (var keyValuePair in right)
            {
                var rightSecurityScheme = _rightComponents.SecuritySchemes[keyValuePair.Key.Scheme];
                if (leftSecurityScheme.Type == rightSecurityScheme.Type)
                {
                    switch (leftSecurityScheme.Type)
                    {
                        case SecuritySchemeType.ApiKey:
                            if (leftSecurityScheme.Name == rightSecurityScheme.Name)
                            {
                                found.Add(keyValuePair.Key, keyValuePair.Value);
                                return found;
                            }

                            break;
                        case SecuritySchemeType.Http:
                        case SecuritySchemeType.OAuth2:
                        case SecuritySchemeType.OpenIdConnect:
                            found.Add(keyValuePair.Key, keyValuePair.Value);
                            return found;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            return found;
        }

        public ChangedSecurityRequirementBO Diff(
            OpenApiSecurityRequirement left, OpenApiSecurityRequirement right, DiffContextBO context)
        {
            var changedSecurityRequirement =
                new ChangedSecurityRequirementBO(left, right != null ? GetCopy(right) : null);

            left ??= new OpenApiSecurityRequirement();
            right ??= new OpenApiSecurityRequirement();

            foreach (var (key, value) in left)
            {
                var rightSec = Contains(right, key.Scheme);
                if (rightSec.IsNullOrEmpty())
                {
                    changedSecurityRequirement.Missing.Add(key, value);
                }
                else
                {
                    var rightSchemeRef = rightSec.Keys.First();
                    right.Remove(rightSchemeRef);
                    var diff =
                        _openApiDiff
                            .SecuritySchemeDiff
                            .diff(
                                key,
                                value,
                                rightSchemeRef,
                                rightSec[rightSchemeRef],
                                context);
                    if (diff != null)
                        changedSecurityRequirement.Changed.Add(diff);
                }
            }

            foreach (var (key, value) in right)
            {
                changedSecurityRequirement.Increased.Add(key, value);
            }

            return ChangedUtils.IsChanged(changedSecurityRequirement);
        }
    }
}
