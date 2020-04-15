using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;
using openapi_diff.Extensions;
using openapi_diff.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace openapi_diff.compare
{
    public class PathsDiff
    {
        private const string RegexPath = "\\{([^/]+)\\}";
        private readonly OpenApiDiff _openApiDiff;

        public PathsDiff(OpenApiDiff openApiDiff)
        {
            _openApiDiff = openApiDiff;
        }

        public ChangedPathsBO Diff(Dictionary<string, OpenApiPathItem> left, Dictionary<string, OpenApiPathItem> right)
        {
            var changedPaths = new ChangedPathsBO(left, right);

            foreach (var (key, value) in right)
            {
                changedPaths.Increased.Add(key, value);
            }

            foreach (var (key, value) in left)
            {
                var template = NormalizePath(key);
                var result = right.Keys.FirstOrDefault(x => NormalizePath(x) == template);

                if (result != null)
                {
                    if (!changedPaths.Increased.ContainsKey(result))
                        throw new ArgumentException($"Two path items have the same signature: {template}");
                    var rightPath = changedPaths.Increased[result];
                    changedPaths.Increased.Remove(result);
                    var paramsDict = new Dictionary<string, string>();
                    if (key != result)
                    {
                        var oldParams = ExtractParameters(key);
                        var newParams = ExtractParameters(result);
                        for (var i = oldParams.Count - 1; i >= 0; i--)
                        {
                            paramsDict.Add(oldParams[i], newParams[i]);
                        }
                    }
                    var context = new DiffContextBO();
                    context.setUrl(key);
                    context.setParameters(paramsDict);

                    var diff = _openApiDiff
                        .PathDiff
                        .Diff(value, rightPath, context);

                    if (diff != null)
                        changedPaths.Changed.Add(result, diff);
                }
                else
                {
                    changedPaths.Missing.Add(key, value);
                }
            }

            return ChangedUtils.IsChanged(changedPaths);
        }

        private static string NormalizePath(string path)
        {
            return Regex.Replace(path, RegexPath, "{}");
        }

        private static List<string> ExtractParameters(string path)
        {
            var paramsList = new List<string>();
            var reg = new Regex(RegexPath);
            var matches = reg.Matches(path);
            if (!matches.IsNullOrEmpty())
            {
                foreach (Match m in matches)
                    paramsList.Add(m.Groups[1].Value);
            }
            return paramsList;
        }

        public static OpenApiPaths ValOrEmpty(OpenApiPaths path)
        {
            return path ?? new OpenApiPaths();
        }
    }
}
