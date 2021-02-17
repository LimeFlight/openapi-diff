using LimeFlight.OpenAPI.Diff.Enums;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LimeFlight.OpenAPI.Diff.Action.Utils
{
    public static class LabelUtil
    {
        public static async Task CreateIfNotExist(GitHubClient github, string owner, string name)
        {
            var allLabels = await github.Issue.Labels.GetAllForRepository(owner, name);
            var labels = GetLabels();
            foreach (var label in labels.Where(label => allLabels.All(x => x.Name != label.Name)))
            {
                await github.Issue.Labels.Create(owner, name, label);
            }
        }

        public static string GetLabelForDiffResult(DiffResultEnum diffResult)
        {
            return diffResult switch
            {
                DiffResultEnum.NoChanges => GetNoChangesLabel().Name,
                DiffResultEnum.Metadata => GetMetadataLabel().Name,
                DiffResultEnum.Compatible => GetCompatibleLabel().Name,
                DiffResultEnum.Unknown => GetUnknownLabel().Name,
                DiffResultEnum.Incompatible => GetIncompatibleLabel().Name,
                _ => throw new ArgumentOutOfRangeException(nameof(diffResult), diffResult, null)
            };
        }

        public static NewLabel GetNoChangesLabel()
        {
            return new NewLabel("OAS:NoChanges", DiffResultEnum.NoChanges.GetColorCodeForDiffResult())
            {
                Description = "No OpenAPI Specification changes"
            };
        }
        public static NewLabel GetMetadataLabel()
        {
            return new NewLabel("OAS:Metadata", DiffResultEnum.Metadata.GetColorCodeForDiffResult())
            {
                Description = "Metadata OpenAPI Specification changes"
            };
        }
        public static NewLabel GetCompatibleLabel()
        {
            return new NewLabel("OAS:Compatible", DiffResultEnum.Compatible.GetColorCodeForDiffResult())
            {
                Description = "Compatible OpenAPI Specification changes"
            };
        }
        public static NewLabel GetUnknownLabel()
        {
            return new NewLabel("OAS:Unknown", DiffResultEnum.Unknown.GetColorCodeForDiffResult())
            {
                Description = "Unknown OpenAPI Specification changes"
            };
        }
        public static NewLabel GetIncompatibleLabel()
        {
            return new NewLabel("OAS:Incompatible", DiffResultEnum.Incompatible.GetColorCodeForDiffResult())
            {
                Description = "Incompatible OpenAPI Specification changes"
            };
        }

        public static string GetColorCodeForDiffResult(this DiffResultEnum diffResult)
        {
            return diffResult switch
            {
                DiffResultEnum.NoChanges => "cccccc",
                DiffResultEnum.Metadata => "1d76db",
                DiffResultEnum.Compatible => "0e8a16",
                DiffResultEnum.Unknown => "f9d0c4",
                DiffResultEnum.Incompatible => "d93f0b",
                _ => throw new ArgumentOutOfRangeException(nameof(diffResult), diffResult, null),
            };
        }

        private static IEnumerable<NewLabel> GetLabels()
        {
            return new List<NewLabel>
            {
                GetNoChangesLabel(),
                GetMetadataLabel(),
                GetCompatibleLabel(),
                GetUnknownLabel(),
                GetIncompatibleLabel()
            };
        }
    }
}