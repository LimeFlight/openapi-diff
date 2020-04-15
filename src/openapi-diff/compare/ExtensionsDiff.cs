using openapi_diff.BusinessObjects;
using openapi_diff.DTOs;
using openapi_diff.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Interfaces;

namespace openapi_diff.compare
{
    public class ExtensionsDiff
    {
        private readonly OpenApiDiff _openApiDiff;
        private readonly IEnumerable<IExtensionDiff> _extensions;

        public ExtensionsDiff(OpenApiDiff openApiDiff, IEnumerable<IExtensionDiff> extensions)
        {
            _openApiDiff = openApiDiff;
            _extensions = extensions;
        }

        public bool IsParentApplicable(TypeEnum type, object parent, IDictionary<string, IOpenApiExtension> extensions, DiffContextBO context)
        {
            if (extensions.Count == 0)
            {
                return true;
            }

            return extensions.Select(x => ExecuteExtension(x.Key, y => y
                    .IsParentApplicable(type, parent, x.Value, context)))
                .All(x => x);
        }

        public IExtensionDiff GetExtensionDiff(string name)
        {
            return _extensions.FirstOrDefault(x => $"x-{x.GetName()}" == name);
        }

        public T ExecuteExtension<T>(string name, Func<ExtensionDiff, T> predicate)
        {
            return predicate(GetExtensionDiff(name).SetOpenApiDiff(_openApiDiff));
        }

        public ChangedExtensionsBO Diff(IDictionary<string, IOpenApiExtension> left, IDictionary<string, IOpenApiExtension> right)
        {
            return Diff(left, right, null);
        }

        public ChangedExtensionsBO Diff(IDictionary<string, IOpenApiExtension> left, IDictionary<string, IOpenApiExtension> right, DiffContextBO context)
        {
            left = ((Dictionary<string, IOpenApiExtension>)left).CopyDictionary();
            right = ((Dictionary<string, IOpenApiExtension>)right).CopyDictionary();
            var changedExtensions = new ChangedExtensionsBO((Dictionary<string, IOpenApiExtension>)left, ((Dictionary<string, IOpenApiExtension>)right).CopyDictionary(), context);
            foreach (var (key, value) in left)
            {
                if (right.ContainsKey(key))
                {
                    var rightValue = right[key];
                    right.Remove(key);
                    var changed = ExecuteExtensionDiff(key, ChangeBO<object>.Changed(value, rightValue), context);
                    if (changed.IsDifferent())
                        changedExtensions.Changed.Add(key, changed);
                }
                else
                {
                    var changed = ExecuteExtensionDiff(key, ChangeBO<object>.Removed(value), context);
                    if (changed.IsDifferent())
                        changedExtensions.Missing.Add(key, changed);
                }
            }

            foreach (var (key, value) in right)
            {
                var changed = ExecuteExtensionDiff(key, ChangeBO<object>.Added(value), context);
                if (changed.IsDifferent())
                    changedExtensions.Increased.Add(key, changed);
            }

            return ChangedUtils.IsChanged(changedExtensions);
        }

        private ChangedBO ExecuteExtensionDiff<T>(string name, ChangeBO<T> change, DiffContextBO context)
        where T : class
        {
            return ExecuteExtension(name, x => x.SetOpenApiDiff(_openApiDiff)
                .Diff(change, context));
        }
    }
}
