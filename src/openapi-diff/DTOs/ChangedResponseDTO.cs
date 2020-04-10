namespace openapi_diff.DTOs
{
    public class ChangedResponseDTO
    {
        public ChangedMetadataDTO Description { get; set; }
        public ChangedHeadersDTO Headers { get; set; }
        public ChangedContentDTO Content { get; set; }
        public ChangedExtensionsDTO Extensions { get; set; }
    }
}
