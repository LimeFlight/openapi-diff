using System;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using System.Collections.Generic;
using System.Linq;

namespace openapi_diff.compare
{
    public class OpenApiDiff
    {
        public const string SwaggerVersionV2 = "2.0";

        private readonly ILogger<OpenApiDiff> _logger;

        public PathsDiff PathsDiff { get; set; }
        public PathDiff PathDiff { get; set; }
        public SchemaDiff SchemaDiff { get; set; }
        public ContentDiff ContentDiff { get; set; }
        public ParametersDiff ParametersDiff { get; set; }
        public ParameterDiff ParameterDiff { get; set; }
        public RequestBodyDiff RequestBodyDiff { get; set; }
        public ResponseDiff ResponseDiff { get; set; }
        public HeadersDiff HeadersDiff { get; set; }
        public HeaderDiff HeaderDiff { get; set; }
        public ApiResponseDiff APIResponseDiff { get; set; }
        public OperationDiff OperationDiff { get; set; }
        public SecurityRequirementsDiff SecurityRequirementsDiff { get; set; }
        public SecurityRequirementDiff SecurityRequirementDiff { get; set; }
        public SecuritySchemeDiff SecuritySchemeDiff { get; set; }
        public OAuthFlowsDiff OAuthFlowsDiff { get; set; }
        public OAuthFlowDiff OAuthFlowDiff { get; set; }
        public ExtensionsDiff ExtensionsDiff { get; set; }
        public MetadataDiff MetadataDiff { get; set; }

        public OpenApiDocument OldSpecOpenApi { get; set; }
        public OpenApiDocument NewSpecOpenApi { get; set; }
        public List<EndpointBO> NewEndpoints { get; set; }
        public List<EndpointBO> MissingEndpoints { get; set; }
        public List<ChangedOperationBO> ChangedOperations { get; set; }
        public ChangedExtensionsBO ChangedExtensions { get; set; }

        public OpenApiDiff(OpenApiDocument oldSpecOpenApi, OpenApiDocument newSpecOpenApi, IEnumerable<IExtensionDiff> extensions, ILogger<OpenApiDiff> logger)
        {
            _logger = logger;
            OldSpecOpenApi = oldSpecOpenApi;
            NewSpecOpenApi = newSpecOpenApi;
            if (null == oldSpecOpenApi || null == newSpecOpenApi)
            {
                throw new Exception("one of the old or new object is null");
            }
            InitializeFields(extensions);
        }

        public static ChangedOpenApiBO Compare(OpenApiDocument oldSpecOpenApi, OpenApiDocument newSpecOpenApi, IEnumerable<IExtensionDiff> extensions, ILogger<OpenApiDiff> logger)
        {
            return new OpenApiDiff(oldSpecOpenApi, newSpecOpenApi, extensions, logger).Compare();
        }

        private void InitializeFields(IEnumerable<IExtensionDiff> extensions)
        {
            PathsDiff = new PathsDiff(this);
            PathDiff = new PathDiff(this);
            SchemaDiff = new SchemaDiff(this);
            ContentDiff = new ContentDiff(this);
            ParametersDiff = new ParametersDiff(this);
            ParameterDiff = new ParameterDiff(this);
            RequestBodyDiff = new RequestBodyDiff(this);
            ResponseDiff = new ResponseDiff(this);
            HeadersDiff = new HeadersDiff(this);
            HeaderDiff = new HeaderDiff(this);
            APIResponseDiff = new ApiResponseDiff(this);
            OperationDiff = new OperationDiff(this);
            SecurityRequirementsDiff = new SecurityRequirementsDiff(this);
            SecurityRequirementDiff = new SecurityRequirementDiff(this);
            SecuritySchemeDiff = new SecuritySchemeDiff(this);
            OAuthFlowsDiff = new OAuthFlowsDiff(this);
            OAuthFlowDiff = new OAuthFlowDiff(this);
            ExtensionsDiff = new ExtensionsDiff(this, extensions);
            MetadataDiff = new MetadataDiff(this);
        }

        private ChangedOpenApiBO Compare()
        {
            preProcess(OldSpecOpenApi);
            preProcess(NewSpecOpenApi);
            var paths =
                PathsDiff.Diff(
                    valOrEmpty(oldSpecOpenApi.getPaths()), valOrEmpty(newSpecOpenApi.getPaths()));
            this.newEndpoints = new ArrayList<>();
            this.missingEndpoints = new ArrayList<>();
            this.changedOperations = new ArrayList<>();
            paths.ifPresent(
                changedPaths-> {
                this.newEndpoints = EndpointUtils.convert2EndpointList(changedPaths.getIncreased());
                this.missingEndpoints = EndpointUtils.convert2EndpointList(changedPaths.getMissing());
                changedPaths
                    .getChanged()
                    .keySet()
                    .forEach(
                        path-> {
                    ChangedPath changedPath = changedPaths.getChanged().get(path);
                    this.newEndpoints.addAll(
                        EndpointUtils.convert2Endpoints(path, changedPath.getIncreased()));
                    this.missingEndpoints.addAll(
                        EndpointUtils.convert2Endpoints(path, changedPath.getMissing()));
                    changedOperations.addAll(changedPath.getChanged());
                });
            });
            getExtensionsDiff()
                .diff(oldSpecOpenApi.getExtensions(), newSpecOpenApi.getExtensions())
                .ifPresent(this::setChangedExtension);
            return getChangedOpenApi();
        }

        private void setChangedExtension(ChangedExtensions changedExtension)
        {
            this.changedExtensions = changedExtension;
        }

        private void preProcess(OpenAPI openApi)
        {
            List<SecurityRequirement> securityRequirements = openApi.getSecurity();

            if (securityRequirements != null)
            {
                List<SecurityRequirement> distinctSecurityRequirements =
                    securityRequirements.stream().distinct().collect(Collectors.toList());
                Map<String, PathItem> paths = openApi.getPaths();
                if (paths != null)
                {
                    paths
                        .values()
                        .forEach(
                            pathItem->
                                pathItem
                                    .readOperationsMap()
                                    .values()
                                    .stream()
                                    .filter(operation->operation.getSecurity() != null)
                                    .forEach(
                                        operation->
                                            operation.setSecurity(
                                                operation
                                                    .getSecurity()
                                                    .stream()
                                                    .distinct()
                                                    .collect(Collectors.toList()))));
                    paths
                        .values()
                        .forEach(
                            pathItem->
                                pathItem
                                    .readOperationsMap()
                                    .values()
                                    .stream()
                                    .filter(operation->operation.getSecurity() == null)
                                    .forEach(operation->operation.setSecurity(distinctSecurityRequirements)));
                }
                openApi.setSecurity(null);
            }
        }

        private ChangedOpenApi getChangedOpenApi()
        {
            return new ChangedOpenApi()
                .setMissingEndpoints(missingEndpoints)
                .setNewEndpoints(newEndpoints)
                .setNewSpecOpenApi(newSpecOpenApi)
                .setOldSpecOpenApi(oldSpecOpenApi)
                .setChangedOperations(changedOperations)
                .setChangedExtensions(changedExtensions);
        }
    }
}
