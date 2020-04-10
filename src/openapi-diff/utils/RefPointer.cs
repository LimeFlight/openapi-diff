using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;
using System;
using System.Collections.Generic;

namespace openapi_diff.utils
{
    public class RefPointer<T>
    {
        public const string BaseRef = "#/components/";
        private readonly RefTypeEnum _refType;

        public RefPointer(RefTypeEnum refType)
        {
            _refType = refType;
        }

        public T ResolveRef(OpenApiComponents components, T t, string reference)
        {
            if (reference != null)
            {
                var refName = GetRefName(reference);
                GetMap(components).TryGetValue(refName, out var result);
                if (result == null)
                {
                    throw new AggregateException($"ref '{reference}' doesn't exist.");
                }
                return result;
            }
            return t;
        }

        private Dictionary<string, T> GetMap(OpenApiComponents components)
        {
            switch (_refType)
            {
                case RefTypeEnum.RequestBodies:
                    return (Dictionary<string, T>)components.RequestBodies;
                case RefTypeEnum.Responses:
                    return (Dictionary<string, T>)components.Responses;
                case RefTypeEnum.Parameters:
                    return (Dictionary<string, T>)components.Parameters;
                case RefTypeEnum.Schemas:
                    return (Dictionary<string, T>)components.Schemas;
                case RefTypeEnum.Headers:
                    return (Dictionary<string, T>)components.Headers;
                case RefTypeEnum.SecuritySchemes:
                    return (Dictionary<string, T>)components.SecuritySchemes;
                default:
                    throw new ArgumentOutOfRangeException("Not mapped for refType: " + _refType);
            }
        }

        public string GetRefName(string reference)
        {
            if (reference == null)
            {
                return null;
            }
            if (_refType == RefTypeEnum.SecuritySchemes)
            {
                return reference;
            }

            var baseRef = GetBaseRefForType(_refType.GetDisplayName());
            if (!reference.StartsWith(baseRef))
            {
                throw new AggregateException("Invalid ref: " + reference);
            }
            return reference.Substring(baseRef.Length);
        }

        private static string GetBaseRefForType(string type)
        {
            return $"{BaseRef}{type}/";
        }
    }
}
