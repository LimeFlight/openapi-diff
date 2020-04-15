using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.Extensions;
using openapi_diff.utils;
using System.Collections.Generic;
using System.Linq;

namespace openapi_diff.compare
{
    public class OperationDiff
    {
        private readonly OpenApiDiff _openApiDiff;

        public OperationDiff(OpenApiDiff openApiDiff)
        {
            this._openApiDiff = openApiDiff;
        }

        public ChangedOperationBO Diff(
            OpenApiOperation oldOperation, OpenApiOperation newOperation, DiffContextBO context)
        {
            var changedOperation =
                new ChangedOperationBO(context.URL, context.Method, oldOperation, newOperation)
                {
                    Summary = _openApiDiff
                        .MetadataDiff
                        .Diff(oldOperation.Summary, newOperation.Summary, context),
                    Description = _openApiDiff
                        .MetadataDiff
                        .Diff(oldOperation.Description, newOperation.Description, context),
                    IsDeprecated = !oldOperation.Deprecated && newOperation.Deprecated
                };

            if (oldOperation.RequestBody != null || newOperation.RequestBody != null)
                changedOperation.RequestBody = _openApiDiff
                    .RequestBodyDiff
                    .Diff(
                        oldOperation.RequestBody, newOperation.RequestBody, context.copyAsRequest());

            var parametersDiff = _openApiDiff
                .ParametersDiff
                .Diff(oldOperation.Parameters.ToList(), newOperation.Parameters.ToList(), context);

            if (parametersDiff != null)
            {
                RemovePathParameters(context.Parameters, parametersDiff);
                changedOperation.Parameters = parametersDiff;
            }


            if (oldOperation.Responses != null || newOperation.Responses != null)
            {
                changedOperation.APIResponses =
                    _openApiDiff
                        .APIResponseDiff
                        .Diff(oldOperation.Responses, newOperation.Responses, context.copyAsResponse());
            }

            if (oldOperation.Security != null || newOperation.Security != null)
            {
                changedOperation.SecurityRequirements =
                    _openApiDiff
                        .SecurityRequirementsDiff
                        .diff(oldOperation.Security, newOperation.Security, context);
            }
            changedOperation.Extensions =
                _openApiDiff
                    .ExtensionsDiff
                    .diff(oldOperation.Extensions, newOperation.Extensions, context);

            return ChangedUtils.IsChanged(changedOperation);
        }

        public void RemovePathParameters(Dictionary<string, string> pathParameters, ChangedParametersBO parameters)
        {
            foreach (var (oldParam, newParam) in pathParameters)
            {
                RemovePathParameter(oldParam, parameters.Missing);
                RemovePathParameter(newParam, parameters.Increased);
            }
        }

        public void RemovePathParameter(string name, List<OpenApiParameter> parameters)
        {
            var openApiParameters = parameters
                .FirstOrDefault(x => x.In == ParameterLocation.Path && x.Name == name);
            if (!parameters.IsNullOrEmpty() && openApiParameters != null)
                parameters.Remove(openApiParameters);
        }
    }
}
