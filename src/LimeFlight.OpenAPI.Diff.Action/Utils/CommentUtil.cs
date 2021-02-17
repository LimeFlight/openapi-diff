using System;
using System.Collections.Generic;
using System.Linq;
using LimeFlight.OpenAPI.Diff.Enums;

namespace LimeFlight.OpenAPI.Diff.Action.Utils
{
    public static class CommentUtil
    {
        private const string IdentifierKey = "openapi-diff-action-identifier-";
        private const string ChangeLevelKey = "openapi-diff-action-changelevel-";

        public static string GetIdentifier(string commentBody)
        {
            if (!commentBody.Contains(IdentifierKey))
                return string.Empty;

            var pFrom = commentBody.IndexOf(IdentifierKey, StringComparison.Ordinal) + IdentifierKey.Length;
            var pTo = commentBody.AllIndexesOf("]").Where(x => x > pFrom).Min();

            return commentBody[pFrom..pTo];
        }

        public static DiffResultEnum GetDiffResult(string commentBody)
        {
            if (!commentBody.Contains(ChangeLevelKey))
                return DiffResultEnum.Unknown;

            var pFrom = commentBody.IndexOf(ChangeLevelKey, StringComparison.Ordinal) + ChangeLevelKey.Length;
            var pTo = commentBody.AllIndexesOf("]").Where(x => x > pFrom).Min();

            if (Enum.TryParse(typeof(DiffResultEnum), commentBody[pFrom..pTo], out var changeLevel))
                return (DiffResultEnum)changeLevel;

            return DiffResultEnum.Unknown;
        }

        private static IEnumerable<int> AllIndexesOf(this string str, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", nameof(value));
            var indexes = new List<int>();
            for (var index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index, StringComparison.Ordinal);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }

        public static string GetCommentMarkdown(string fileName, DiffResultEnum diffResult, string markdown)
        {
            var identifier = GetIdentifierString(fileName);
            var changeLevel = $"<!-- [{ChangeLevelKey}{diffResult}] -->";

            var title = $"## OpenAPI Diff Report for {fileName}";
            var badge = $"![]({GetBadge(diffResult)})";
            const string footer = "---\n" +
                                  "<a href=\"https://github.com/LimeFlight/openapi-diff-action\"><img src=\"https://img.shields.io/static/v1?label=GitHub%20Actions&message=OpenAPI%20Diff%20In%20PR&color=green&logo=github\" /></a>";
            return $"{identifier}\n{changeLevel}\n{title}\n{badge}\n{markdown}\n{footer}";
        }

        private static string GetBadge(DiffResultEnum diffResult)
        {
            const string badge = "https://img.shields.io/badge/OpenAPI-{0}-{1}";

            return diffResult switch
            {
                DiffResultEnum.NoChanges => string.Format(badge, "No%20Changes", diffResult.GetColorCodeForDiffResult()),
                DiffResultEnum.Metadata => string.Format(badge, "Metadata", diffResult.GetColorCodeForDiffResult()),
                DiffResultEnum.Compatible => string.Format(badge, "Compatible", diffResult.GetColorCodeForDiffResult()),
                DiffResultEnum.Unknown => string.Format(badge, "Unknown", diffResult.GetColorCodeForDiffResult()),
                DiffResultEnum.Incompatible => string.Format(badge, "Incompatible",
                    diffResult.GetColorCodeForDiffResult()),
                _ => throw new ArgumentOutOfRangeException(nameof(diffResult), diffResult, null)
            };
        }

        public static string GetIdentifierString(string fileName)
        {
            return $"<!-- [{IdentifierKey}{fileName}] -->";
        }
    }
}
