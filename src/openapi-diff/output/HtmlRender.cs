using DotNet2HTML.Tags;
using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.DTOs;
using openapi_diff.utils;
using System;
using System.Collections.Generic;
using static DotNet2HTML.TagCreator;

namespace openapi_diff.output
{
    public class HtmlRender : IRender
    {
        private readonly string _title;
        private readonly string _linkCss;
        protected static RefPointer<OpenApiSchema> refPointer = new RefPointer<OpenApiSchema>(RefTypeEnum.Schemas);
        protected ChangedOpenApiDTO Diff;

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

        public string Render(ChangedOpenApiDTO diff)
        {
            Diff = diff;

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

        public string RenderHtml(ContainerTag olNwEndpoint, ContainerTag olMissingEndpoint, ContainerTag olDeprecatedEndpoint, ContainerTag olChanged)
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

        private static ContainerTag OlNewEndpoint(List<EndpointDTO> endpoints)
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
        private static ContainerTag OlMissingEndpoint(List<EndpointDTO> endpoints)
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
        private static ContainerTag OlDeprecatedEndpoint(List<EndpointDTO> endpoints)
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
        private static ContainerTag OlChanged(List<ChangedOperationDTO> changedOperations)
        {
            if (null == changedOperations) return Ol();
            var ol = Ol();

            foreach (var changedOperation in changedOperations)
            {
                var pathUrl = changedOperation.PathUrl;
                var method = changedOperation.HttpMethod.ToString();
                var desc = changedOperation.Summary != null ? changedOperation.Summary.Right : "";

                var ulDetail = Ul().WithClass("detail");
            }

            throw new NotImplementedException();
        }
    }
}
