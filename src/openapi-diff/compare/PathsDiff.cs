using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using openapi_diff.BusinessObjects;

namespace openapi_diff.compare
{
    public class PathsDiff
    {
        private const string RegexPath = "\\{([^/]+)\\}";
        private OpenApiDiff _openApiDiff;

        public PathsDiff(OpenApiDiff openApiDiff)
        {
            _openApiDiff = openApiDiff;
        }

        private static string normalizePath(string path)
        {
            return path.Replace(RegexPath, "{}");
        }

        private static List<string> extractParameters(string path)
        {
            var paramsList = new List<string>();
            var pattern = new Regex(RegexPath);
            var matcher = pattern.Match(path);
            while (matcher.Success)
            {
                paramsList.Add(matcher.Groups.Values.FirstOrDefault(x => x.));
            }
            return paramsList;
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
                
            }

            left.keySet()
                .forEach(
                    (String url)-> {
                PathItem leftPath = left.get(url);
                String template = normalizePath(url);
                Optional<String> result =
                    right
                        .keySet()
                        .stream()
                        .filter(s->normalizePath(s).equals(template))
                        .findFirst();
                if (result.isPresent())
                {
                    if (!changedPaths.getIncreased().containsKey(result.get()))
                    {
                        throw new IllegalArgumentException(
                            "Two path items have the same signature: " + template);
                    }
                    PathItem rightPath = changedPaths.getIncreased().remove(result.get());
                    Map < String, String > params = new LinkedHashMap<>();
                    if (!url.equals(result.get()))
                    {
                        List<String> oldParams = extractParameters(url);
                        List<String> newParams = extractParameters(result.get());
                        for (int i = 0; i < oldParams.size(); i++)
                        {
                    params.put(oldParams.get(i), newParams.get(i));
                        }
                    }
                    DiffContext context = new DiffContext();
                    context.setUrl(url);
                    context.setParameters(params);
                    openApiDiff
                        .getPathDiff()
                        .diff(leftPath, rightPath, context)
                        .ifPresent(path->changedPaths.getChanged().put(result.get(), path));
                }
                else
                {
                    changedPaths.getMissing().put(url, leftPath);
                }
            });
            return isChanged(changedPaths);
        }

        public static Paths valOrEmpty(Paths path)
        {
            if (path == null)
            {
                path = new Paths();
            }
            return path;
        }
    }
}
