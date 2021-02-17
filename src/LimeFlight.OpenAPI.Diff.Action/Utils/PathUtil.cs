using System;
using System.IO;

namespace LimeFlight.OpenAPI.Diff.Action.Utils
{
    public static class PathUtil
    {
        public static bool TryGetAbsoluteUri(string path, out Uri absoluteUri)
        {
            absoluteUri = null;
            if (!Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out var tmpUri))
                return false;

            if (!tmpUri.IsAbsoluteUri)
                tmpUri = new Uri(Path.GetFullPath(tmpUri.OriginalString));

            if (!tmpUri.IsAbsoluteUri)
                return false;

            absoluteUri = tmpUri;
            return true;
        }
    }
}
