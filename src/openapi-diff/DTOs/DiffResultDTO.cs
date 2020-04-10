namespace openapi_diff.DTOs
{
    public class DiffResultDTO
    {
        public int Weight { get; }
        public string Value { get; }

        public DiffResultEnum Status { get; set; }

        public DiffResultDTO(string value, int weight)
        {
            Value = value;
            Weight = weight;
        }
    }
}
