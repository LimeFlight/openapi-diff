//using DotNet2HTML.Tags;
//using Microsoft.OpenApi.Models;
//using openapi_diff.BusinessObjects;
//using openapi_diff.DTOs;
//using openapi_diff.utils;
//using System;
//using System.Collections.Generic;
//using static DotNet2HTML.TagCreator;

//namespace openapi_diff.output
//{
//    public class HtmlRender : IRender
//    {
//        private readonly string _title;
//        private readonly string _linkCss;
//        protected static RefPointer<OpenApiSchema> refPointer = new RefPointer<OpenApiSchema>(RefTypeEnum.Schemas);
//        protected ChangedOpenApiBO Diff;

//        public HtmlRender()
//        {
//            _title = "Api Change Log";
//            _linkCss = "http://deepoove.com/swagger-diff/stylesheets/demo.css";
//        }

//        public HtmlRender(string title, string linkCss)
//        {
//            _title = title;
//            _linkCss = linkCss;
//        }

//        public string Render(ChangedOpenApiBO diff)
//        {
//            Diff = diff;

//            var newEndpoints = diff.NewEndpoints;
//            var olNwEndpoint = OlNewEndpoint(newEndpoints);

//            var missingEndpoints = diff.MissingEndpoints;
//            var olMissingEndpoint = OlMissingEndpoint(missingEndpoints);

//            var deprecatedEndpoints = diff.GetDeprecatedEndpoints();
//            var olDeprecatedEndpoint = OlDeprecatedEndpoint(deprecatedEndpoints);

//            var changedOperations = diff.ChangedOperations;
//            var olChanged = OlChanged(changedOperations);

//            return RenderHtml(olNwEndpoint, olMissingEndpoint, olDeprecatedEndpoint, olChanged);
//        }

//        public string RenderHtml(ContainerTag olNwEndpoint, ContainerTag olMissingEndpoint, ContainerTag olDeprecatedEndpoint, ContainerTag olChanged)
//        {
//            ContainerTag html =
//                Html()
//                    .Attr("lang", "en")
//                    .With(
//                        Head()
//                            .With(
//                                Meta()
//                                    .WithCharset("utf-8"), Title(_title), Link()
//                                    .WithRel("stylesheet").WithHref(_linkCss)),
//                        Body()
//                            .With(
//                                Header()
//                                    .With(H1(_title)), Div()
//                                    .WithClass("article")
//                                    .With(
//                                        Div().With(H2("What's New"), Hr(), olNwEndpoint),
//                                        Div().With(H2("What's Deleted"), Hr(), olMissingEndpoint),
//                                        Div().With(H2("What's Deprecated"), Hr(), olDeprecatedEndpoint),
//                                        Div().With(H2("What's Changed"), Hr(), olChanged))));

//            return Document().Render() + html.Render();
//        }

//        private static ContainerTag OlNewEndpoint(IReadOnlyCollection<EndpointBO> endpoints)
//        {
//            if (null == endpoints) return Ol();
//            var ol = Ol();

//            foreach (var endpoint in endpoints)
//            {
//                ol.With(
//                    LiNewEndpoint(endpoint.Method.ToString(), endpoint.PathUrl, endpoint.Summary)
//                );
//            }

//            return ol;
//        }
//        private static ContainerTag LiNewEndpoint(string method, string path, string desc)
//        {
//            return Li().With(Span(method).WithClass(method)).WithText(path + " ").With(Span(desc));
//        }
//        private static ContainerTag OlMissingEndpoint(IReadOnlyCollection<EndpointBO> endpoints)
//        {
//            if (null == endpoints) return Ol();
//            var ol = Ol();

//            foreach (var endpoint in endpoints)
//            {
//                ol.With(
//                    LiMissingEndpoint(
//                        endpoint.Method.ToString(), endpoint.PathUrl, endpoint.Summary));
//            }

//            return ol;
//        }
//        private static ContainerTag LiMissingEndpoint(string method, string path, string desc)
//        {
//            return Li().With(Span(method).WithClass(method), Del().WithText(path)).With(Span(" " + desc));
//        }
//        private static ContainerTag OlDeprecatedEndpoint(IReadOnlyCollection<EndpointBO> endpoints)
//        {
//            if (null == endpoints) return Ol();
//            var ol = Ol();
//            foreach (var endpoint in endpoints)
//            {
//                ol.With(
//                    LiDeprecatedEndpoint(
//                        endpoint.Method.ToString(), endpoint.PathUrl, endpoint.Summary));
//            }

//            return ol;
//        }
//        private static ContainerTag LiDeprecatedEndpoint(string method, string path, string desc)
//        {
//            return Li().With(Span(method).WithClass(method), Del().WithText(path)).With(Span(" " + desc));
//        }
//        private static ContainerTag OlChanged(IReadOnlyCollection<ChangedOperationBO> changedOperations)
//        {
//            if (null == changedOperations) return Ol();
//            var ol = Ol();

//            foreach (var changedOperation in changedOperations)
//            {
//                var pathUrl = changedOperation.PathUrl;
//                var method = changedOperation.HttpMethod.ToString();
//                var desc = changedOperation.Summary != null ? changedOperation.Summary.Right : "";

//                var ulDetail = Ul().WithClass("detail");
//                if (ChangedBO.Result(changedOperation.Parameters).IsDifferent())
//                {
//                    ulDetail.With(
//                        Li().With(H3("Parameters")).With(ul_param(changedOperation.Parameters)));
//                }
//                if (changedOperation.resultRequestBody().isDifferent())
//                {
//                    ul_detail.with(
//                        li().with(h3("Request"))
//                            .with(ul_request(changedOperation.getRequestBody().getContent())));
//                }
//                else
//                {
//                }
//                if (changedOperation.resultApiResponses().isDifferent())
//                {
//                    ul_detail.with(
//                        li().with(h3("Response")).with(ul_response(changedOperation.getApiResponses())));
//                }
//                ol.with(
//                    li().with(span(method).withClass(method))
//                        .withText(pathUrl + " ")
//                        .with(span(desc))
//                        .with(ul_detail));
//            }

//            return ol;
//        }
//        private ContainerTag ul_param(ChangedParametersBO changedParameters)
//        {
//            var addParameters = changedParameters.Increased;
//            var delParameters = changedParameters.Missing;
//            var changed = changedParameters.Changed;
//            var ul = Ul().WithClass("change param");
//            foreach (var openApiParameter in addParameters)
//            {
//                Ul().With(li_addParam(openApiParameter));
//            }

//            for (Parameter param : addParameters)
//            {
//                ul.with(li_addParam(param));
//            }
//            for (ChangedParameter param : changed)
//            {
//                ul.with(li_changedParam(param));
//            }
//            for (Parameter param : delParameters)
//            {
//                ul.with(li_missingParam(param));
//            }
//            return ul;
//        }

//        private ContainerTag li_addParam(Parameter param)
//        {
//            return li().withText("Add " + param.getName() + " in " + param.getIn())
//                .with(
//                    span(null == param.getDescription() ? "" : ("//" + param.getDescription()))
//                        .withClass("comment"));
//        }

//        private ContainerTag li_missingParam(Parameter param)
//        {
//            return li().withClass("missing")
//                .with(span("Delete"))
//                .with(del(param.getName()))
//                .with(span("in ").withText(param.getIn()))
//                .with(
//                    span(null == param.getDescription() ? "" : ("//" + param.getDescription()))
//                        .withClass("comment"));
//        }

//        private ContainerTag li_deprecatedParam(ChangedParameter param)
//        {
//            return li().withClass("missing")
//                .with(span("Deprecated"))
//                .with(del(param.getName()))
//                .with(span("in ").withText(param.getIn()))
//                .with(
//                    span(null == param.getNewParameter().getDescription()
//                            ? ""
//                            : ("//" + param.getNewParameter().getDescription()))
//                        .withClass("comment"));
//        }

//        private ContainerTag li_changedParam(ChangedParameter changeParam)
//        {
//        }
//}
