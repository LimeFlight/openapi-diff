using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace yaos.OpenAPI.Diff.BusinessObjects
{
    public class DiffContextBO
    {
        public string URL { get; private set; }
        public Dictionary<string, string> Parameters { get; private set; }
        public OperationType Method { get; private set; }
        public bool IsResponse { get; private set; }
        public bool IsRequest { get; private set; }

        public bool IsRequired { get; private set; }

        public DiffContextBO()
        {
            Parameters = new Dictionary<string, string>();
            IsResponse = false;
            IsRequest = true;
        }

        public DiffContextBO CopyWithMethod(OperationType method)
        {
            return copy().setMethod(method);
        }

        public DiffContextBO copyWithRequired(bool required)
        {
            return copy().setRequired(required);
        }

        public DiffContextBO copyAsRequest()
        {
            return copy().setRequest();
        }

        public DiffContextBO copyAsResponse()
        {
            return copy().setResponse();
        }

        private DiffContextBO setRequest()
        {
            IsRequest = true;
            IsResponse = false;
            return this;
        }

        private DiffContextBO setResponse()
        {
            IsResponse = true;
            IsRequest = false;
            return this;
        }

        public DiffContextBO setUrl(string url)
        {
            URL = url;
            return this;
        }

        private DiffContextBO setMethod(OperationType method)
        {
            Method = method;
            return this;
        }

        private DiffContextBO copy()
        {
            var context = new DiffContextBO
            {
                URL = URL,
                Parameters = Parameters,
                Method = Method,
                IsResponse = IsResponse,
                IsRequest = IsRequest,
                IsRequired = IsRequired
            };
            return context;
        }

        public DiffContextBO setParameters(Dictionary<string, string> parameters)
        {
            Parameters = parameters;
            return this;
        }

        private DiffContextBO setRequired(bool required)
        {
            IsRequired = required;
            return this;
        }

        public override bool Equals(object o)
        {
            if (this == o) return true;

            if (o == null || GetType() != o.GetType()) return false;

            var that = (DiffContextBO)o;

            return IsResponse.Equals(that.IsResponse)
                   && IsRequest.Equals(that.IsRequest)
                   && URL.Equals(that.URL)
                   && Parameters.Equals(that.Parameters)
                   && Method.Equals(that.Method)
                   && IsRequired.Equals(that.IsRequired);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(URL, Parameters, Method, IsResponse, IsRequest, IsRequired);
        }
    }
}
