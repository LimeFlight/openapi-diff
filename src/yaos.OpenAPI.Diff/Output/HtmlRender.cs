using System.Collections.Generic;
using DotNet2HTML.Tags;
using Microsoft.OpenApi.Models;
using yaos.OpenAPI.Diff.BusinessObjects;
using yaos.OpenAPI.Diff.Enums;
using yaos.OpenAPI.Diff.Extensions;
using yaos.OpenAPI.Diff.Utils;
using static DotNet2HTML.TagCreator;

namespace yaos.OpenAPI.Diff.Output
{
    public class HtmlRender : IRender
    {
        private readonly string _title;
        private readonly string _linkCss;
        protected static RefPointer<OpenApiSchema> RefPointer = new RefPointer<OpenApiSchema>(RefTypeEnum.Schemas);
        private static ChangedOpenApiBO _diff;

        public HtmlRender()
        {
            _title = "Api Change Log";
            _linkCss = "http://deepoove.com/swagger-diff/stylesheets/demo.css";
        }

        public HtmlRender(string title, string linkCss)
        {
            _title = title;
            _linkCss = linkCss;
        }

        public string Render(ChangedOpenApiBO diff)
        {
            _diff = diff;

            var newEndpoints = diff.NewEndpoints;
            var olNwEndpoint = OlNewEndpoint(newEndpoints);

            var missingEndpoints = diff.MissingEndpoints;
            var olMissingEndpoint = OlMissingEndpoint(missingEndpoints);

            var deprecatedEndpoints = diff.GetDeprecatedEndpoints();
            var olDeprecatedEndpoint = OlDeprecatedEndpoint(deprecatedEndpoints);

            var changedOperations = diff.ChangedOperations;
            var olChanged = OlChanged(changedOperations);

            return RenderHtml(olNwEndpoint, olMissingEndpoint, olDeprecatedEndpoint, olChanged);
        }

        public string RenderHtml(ContainerTag olNwEndpoint, ContainerTag olMissingEndpoint,
            ContainerTag olDeprecatedEndpoint, ContainerTag olChanged)
        {
            ContainerTag html =
                Html()
                    .Attr("lang", "en")
                    .With(
                        Head()
                            .With(
                                Meta()
                                    .WithCharset("utf-8"), Title(_title), Link()
                                    .WithRel("stylesheet").WithHref(_linkCss)),
                        Body()
                            .With(
                                Header()
                                    .With(H1(_title)), Div()
                                    .WithClass("article")
                                    .With(
                                        Div().With(H2("What's New"), Hr(), olNwEndpoint),
                                        Div().With(H2("What's Deleted"), Hr(), olMissingEndpoint),
                                        Div().With(H2("What's Deprecated"), Hr(), olDeprecatedEndpoint),
                                        Div().With(H2("What's Changed"), Hr(), olChanged))));

            return Document().Render() + html.Render();
        }

        private static ContainerTag OlNewEndpoint(IReadOnlyCollection<EndpointBO> endpoints)
        {
            if (null == endpoints) return Ol();
            var ol = Ol();

            foreach (var endpoint in endpoints)
            {
                ol.With(
                    LiNewEndpoint(endpoint.Method.ToString(), endpoint.PathUrl, endpoint.Summary)
                );
            }

            return ol;
        }

        private static ContainerTag LiNewEndpoint(string method, string path, string desc)
        {
            return Li().With(Span(method).WithClass(method)).WithText(path + " ").With(Span(desc));
        }

        private static ContainerTag OlMissingEndpoint(IReadOnlyCollection<EndpointBO> endpoints)
        {
            if (null == endpoints) return Ol();
            var ol = Ol();

            foreach (var endpoint in endpoints)
            {
                ol.With(
                    LiMissingEndpoint(
                        endpoint.Method.ToString(), endpoint.PathUrl, endpoint.Summary));
            }

            return ol;
        }

        private static ContainerTag LiMissingEndpoint(string method, string path, string desc)
        {
            return Li().With(Span(method).WithClass(method), Del().WithText(path)).With(Span(" " + desc));
        }

        private static ContainerTag OlDeprecatedEndpoint(IReadOnlyCollection<EndpointBO> endpoints)
        {
            if (null == endpoints) return Ol();
            var ol = Ol();
            foreach (var endpoint in endpoints)
            {
                ol.With(
                    LiDeprecatedEndpoint(
                        endpoint.Method.ToString(), endpoint.PathUrl, endpoint.Summary));
            }

            return ol;
        }

        private static ContainerTag LiDeprecatedEndpoint(string method, string path, string desc)
        {
            return Li().With(Span(method).WithClass(method), Del().WithText(path)).With(Span(" " + desc));
        }

        private static ContainerTag OlChanged(IReadOnlyCollection<ChangedOperationBO> changedOperations)
        {
            if (null == changedOperations) return Ol();
            var ol = Ol();

            foreach (var changedOperation in changedOperations)
            {
                var pathUrl = changedOperation.PathUrl;
                var method = changedOperation.HttpMethod.ToString();
                var desc = changedOperation.Summary != null ? changedOperation.Summary.Right : "";

                var ulDetail = Ul().WithClass("detail");
                if (ChangedBO.Result(changedOperation.Parameters).IsDifferent())
                {
                    ulDetail.With(
                        Li().With(H3("Parameters")).With(ul_param(changedOperation.Parameters)));
                }

                if (changedOperation.ResultRequestBody().IsDifferent())
                {
                    ulDetail.With(
                        Li().With(H3("Request"))
                            .With(ul_request(changedOperation.RequestBody.Content)));
                }

                if (changedOperation.ResultApiResponses().IsDifferent())
                {
                    ulDetail.With(
                        Li().With(H3("Response")).With(ul_response(changedOperation.APIResponses)));
                }

                ol.With(
                    Li().With(Span(method).WithClass(method))
                        .WithText(pathUrl + " ")
                        .With(Span(desc))
                        .With(ulDetail));
            }

            return ol;
        }

        private static ContainerTag ul_param(ChangedParametersBO changedParameters)
        {
            var addParameters = changedParameters.Increased;
            var delParameters = changedParameters.Missing;
            var changed = changedParameters.Changed;
            var ul = Ul().WithClass("change param");
            foreach (var parameter in addParameters)
            {
                Ul().With(li_addParam(parameter));
            }

            foreach (var parameter in changed)
            {
                ul.With(li_changedParam(parameter));

            }

            foreach (var parameter in delParameters)
            {
                ul.With(li_missingParam(parameter));
            }

            return ul;
        }

        private static ContainerTag ul_request(ChangedContentBO changedContent)
        {
            var ul = Ul().WithClass("change request-body");
            if (changedContent != null)
            {
                foreach (var (propName, value) in changedContent.Increased)
                {
                    ul.With(li_addRequest(propName, value));
                }

                foreach (var (propName, value) in changedContent.Increased)
                {
                    ul.With(li_addRequest(propName, value));
                }

                foreach (var (propName, value) in changedContent.Missing)
                {
                    ul.With(li_missingRequest(propName, value));
                }

                foreach (var (propName, value) in changedContent.Changed)
                {
                    ul.With(li_changedRequest(propName, value));
                }
            }

            return ul;
        }

        private static ContainerTag ul_response(ChangedAPIResponseBO changedApiResponse)
        {
            var addResponses = changedApiResponse.Increased;
            var delResponses = changedApiResponse.Missing;
            var changedResponses = changedApiResponse.Changed;
            var ul = Ul().WithClass("change response");

            foreach (var (propName, value) in addResponses)
            {
                ul.With(li_addResponse(propName, value));
            }

            foreach (var (propName, value) in delResponses)
            {
                ul.With(li_missingResponse(propName, value));
            }

            foreach (var (propName, value) in changedResponses)
            {
                ul.With(li_changedResponse(propName, value));
            }

            return ul;
        }

        private static ContainerTag li_addResponse(string name, OpenApiResponse response)
        {
            return Li().WithText($"New response : {name}")
                .With(
                    Span(null == response.Description ? "" : "//" + response.Description)
                        .WithClass("comment"));
        }

        private static ContainerTag li_missingResponse(string name, OpenApiResponse response)
        {
            return Li().WithText($"Deleted response : {name}")
                .With(
                    Span(null == response.Description ? "" : "//" + response.Description)
                        .WithClass("comment"));
        }

        private static ContainerTag li_changedResponse(string name, ChangedResponseBO response)
        {
            return Li().WithText($"Changed response : {name}")
                .With(
                    Span(response.NewApiResponse?.Description == null
                            ? ""
                            : "//" + response.NewApiResponse.Description)
                        .WithClass("comment"))
                .With(ul_request(response.Content));
        }

        private static ContainerTag li_addRequest(string name, OpenApiMediaType request)
        {
            return Li().WithText($"New body: {name}");
        }

        private static ContainerTag li_missingRequest(string name, OpenApiMediaType request)
        {
            return Li().WithText($"Deleted body: {name}");
        }

        private static ContainerTag li_changedRequest(string name, ChangedMediaTypeBO request)
        {
            var li =
                Li().With(div_changedSchema(request.Schema))
                    .WithText($"Changed body: {name}");
            if (request.IsIncompatible())
            {
                Incompatibilities(li, request.Schema);
            }

            return li;
        }


        private static void Incompatibilities(ContainerTag output, ChangedSchemaBO schema)
        {
            Incompatibilities(output, "", schema);
        }

        private static void Incompatibilities(ContainerTag output, string propName, ChangedSchemaBO schema)
        {
            if (schema.Items != null)
            {
                Items(output, propName, schema.Items);
            }

            if (schema.IsCoreChanged().DiffResult == DiffResultEnum.Incompatible && schema.IsChangedType)
            {
                var type = schema.OldSchema.Type + " -> " + schema.NewSchema.Type;
                Property(output, propName, "Changed property type", type);
            }

            var prefix = propName.IsNullOrEmpty() ? "" : propName + ".";
            Properties(output, prefix, "Missing property", schema.MissingProperties);
            foreach (var (name, property) in schema
                .ChangedProperties)
            {
                Incompatibilities(output, prefix + name, property);
            }
        }
        
        private static void Properties(
            ContainerTag output,
            string propPrefix,
            string title,
            Dictionary<string, OpenApiSchema> properties)
        {
            if (properties != null)
            {
                foreach (var (key, value) in properties)
                {
                    Property(output, propPrefix + key, title, Resolve(value));
                }
            }
        }

        protected static void Property(ContainerTag output, string name, string title, string type)
        {
            output.With(P($"{title}: {name} ({type})").WithClass("missing"));
        }

        protected static void Property(ContainerTag output, string name, string title, OpenApiSchema schema)
        {
            Property(output, name, title, schema.Type);
        }

        protected static OpenApiSchema Resolve(OpenApiSchema schema)
        {
            return RefPointer.ResolveRef(
                _diff.NewSpecOpenApi.Components, schema, schema.Reference?.ReferenceV3);
        }

        private static void Items(ContainerTag output, string propName, ChangedSchemaBO schema)
        {
            Incompatibilities(output, propName + "[n]", schema);
        }

        private static ContainerTag div_changedSchema(ChangedSchemaBO schema)
        {
            var div = Div();
            div.With(H3("Schema" + (schema.IsIncompatible() ? " incompatible" : "")));
            return div;
        }

        private static ContainerTag li_addParam(OpenApiParameter param)
        {
            return Li().WithText("Add " + param.Name + " in " + param.In)
                .With(
                    Span(null == param.Description ? "" : ("//" + param.Description))
                        .WithClass("comment"));
        }

        private static ContainerTag li_missingParam(OpenApiParameter param)
        {
            return Li().WithClass("missing")
                .With(Span("Delete"))
                .With(Del(param.Name))
                .With(Span("in ").WithText(param.In.ToString()))
                .With(
                    Span(null == param.Description ? "" : "//" + param.Description)
                        .WithClass("comment"));
        }

        private static ContainerTag li_deprecatedParam(ChangedParameterBO param)
        {
            return Li().WithClass("missing")
                .With(Span("Deprecated"))
                .With(Del(param.Name))
                .With(Span("in ").WithText(param.In.ToString()))
                .With(
                    Span(null == param.NewParameter.Description
                            ? ""
                            : "//" + param.NewParameter.Description)
                        .WithClass("comment"));
        }

        private static ContainerTag li_changedParam(ChangedParameterBO changeParam)
        {
            if (changeParam.IsDeprecated)
            {
                return li_deprecatedParam(changeParam);
            }

            var changeRequired = changeParam.IsChangeRequired;
            var changeDescription = changeParam.Description.IsDifferent();
            var rightParam = changeParam.NewParameter;
            var leftParam = changeParam.NewParameter;
            var li = Li().WithText(changeParam.Name + " in " + changeParam.In);
            if (changeRequired)
            {
                li.WithText(" change into " + (rightParam.Required ? "required" : "not required"));
            }

            if (changeDescription)
            {
                li.WithText(" Notes ")
                    .With(Del(leftParam.Description).WithClass("comment"))
                    .WithText(" change into ")
                    .With(Span(rightParam.Description).WithClass("comment"));
            }

            return li;
        }
    }
}
