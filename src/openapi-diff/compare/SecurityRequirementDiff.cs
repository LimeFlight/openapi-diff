using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;

namespace openapi_diff.compare
{
    public class SecurityRequirementDiff
    {
        private OpenApiDiff openApiDiff;
        private OpenApiComponents leftComponents;
        private OpenApiComponents rightComponents;

        public SecurityRequirementDiff(OpenApiDiff openApiDiff)
        {
            this.openApiDiff = openApiDiff;
            leftComponents = openApiDiff.OldSpecOpenApi?.Components;
            rightComponents = openApiDiff.NewSpecOpenApi?.Components;
        }
        public static OpenApiSecurityRequirement GetCopy(Dictionary<OpenApiSecurityScheme, List<string>> right)
        {
            var newSecurityRequirement = new OpenApiSecurityRequirement();
            foreach (var (key, value) in right)
            {
                newSecurityRequirement.Add(key, value);
            }
            
            return newSecurityRequirement;
        }

        private LinkedHashMap<String, List<String>> contains(
            SecurityRequirement right, String schemeRef)
        {
            SecurityScheme leftSecurityScheme = leftComponents.SecuritySchemes().(schemeRef);
            LinkedHashMap<String, List<String>> found = new LinkedHashMap<>();

            for (Map.Entry<String, List<String>> entry : right.entrySet())
            {
                SecurityScheme rightSecurityScheme = rightComponents.SecuritySchemes().(entry.Key());
                if (leftSecurityScheme.Type() == rightSecurityScheme.Type())
                {
                    switch (leftSecurityScheme.Type())
                    {
                        case APIKEY:
                            if (leftSecurityScheme.Name().equals(rightSecurityScheme.Name()))
                            {
                                found.put(entry.Key(), entry.Value());
                                return found;
                            }
                            break;

                        case OAUTH2:
                        case HTTP:
                        case OPENIDCONNECT:
                            found.put(entry.Key(), entry.Value());
                            return found;
                    }
                }
            }

            return found;
        }

        public ChangedSecurityRequirementBO Diff(
            OpenApiSecurityRequirement left, OpenApiSecurityRequirement right, DiffContextBO context)
        {
            var newRight = new OpenApiSecurityRequirement[] { };
            right.
            right?.CopyTo(newRight);
            right = newRight.ToList();

            var changedSecurityRequirement =
                new ChangedSecurityRequirementBO(left, right != null ? getCopy(right) : null);

            left = left == null ? new SecurityRequirement() : left;
            right = right == null ? new SecurityRequirement() : right;

            for (String leftSchemeRef : left.keySet())
            {
                LinkedHashMap<String, List<String>> rightSec = contains(right, leftSchemeRef);
                if (rightSec.isEmpty())
                {
                    changedSecurityRequirement.addMissing(leftSchemeRef, left.(leftSchemeRef));
                }
                else
                {
                    String rightSchemeRef = rightSec.keySet().stream().findFirst().();
                    right.remove(rightSchemeRef);
                    Optional<ChangedSecurityScheme> diff =
                        openApiDiff
                            .SecuritySchemeDiff()
                            .diff(
                                leftSchemeRef,
                                left.(leftSchemeRef),
                                rightSchemeRef,
                                rightSec.(rightSchemeRef),
                                context);
                    diff.ifPresent(changedSecurityRequirement::addChanged);
                }
            }
            right.forEach(changedSecurityRequirement::addIncreased);

            return isChanged(changedSecurityRequirement);
        }
    }
}
