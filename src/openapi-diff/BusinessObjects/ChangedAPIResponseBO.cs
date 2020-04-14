using Microsoft.OpenApi.Models;
using openapi_diff.DTOs;
using openapi_diff.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace openapi_diff.BusinessObjects
{
    public class ChangedAPIResponseBO : ComposedChangedBO
    {
        private readonly OpenApiResponses _oldApiResponses;
        private readonly OpenApiResponses _newApiResponses;
        private readonly DiffContextBO _context;
        
        public Dictionary<string, OpenApiResponse> Increased { get; set; }
        public Dictionary<string, OpenApiResponse> Missing { get; set; }
        public Dictionary<string, ChangedResponseBO> Changed { get; set; }
        public ChangedExtensionsBO Extensions { get; set; }

        public ChangedAPIResponseBO(OpenApiResponses oldApiResponses, OpenApiResponses newApiResponses, DiffContextBO context)
        {
            _oldApiResponses = oldApiResponses;
            _newApiResponses = newApiResponses;
            _context = context;
            Increased = new Dictionary<string, OpenApiResponse>();
            Missing = new Dictionary<string, OpenApiResponse>();
            Changed = new Dictionary<string, ChangedResponseBO>();
        }

        public override List<ChangedBO> GetChangedElements()
        {
            return new List<ChangedBO>(Changed.Values.ToList()) {Extensions};
        }

        public override DiffResultBO IsCoreChanged()
        {
            if (Increased.IsNullOrEmpty() && Missing.IsNullOrEmpty())
            {
                return new DiffResultBO(DiffResultEnum.NoChanges);
            }
            if (!Increased.IsNullOrEmpty() && Missing.IsNullOrEmpty())
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}
