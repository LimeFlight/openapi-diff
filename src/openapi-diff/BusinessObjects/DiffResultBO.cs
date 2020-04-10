using openapi_diff.DTOs;

namespace openapi_diff.BusinessObjects
{
    public static class DiffResultBO
    {
        public static bool IsUnchanged(this DiffResultDTO diffResult)
        {
            return diffResult.Weight == 0;
        }

        public static bool IsDifferent(this DiffResultDTO diffResult)
        {
            return diffResult.Weight > 0;
        }

        public static bool IsIncompatible(this DiffResultDTO diffResult)
        {
            return diffResult.Weight > 2;
        }

        public static bool IsCompatible(this DiffResultDTO diffResult)
        {
            return diffResult.Weight <= 2;
        }

        public static bool IsMetaChanged(this DiffResultDTO diffResult)
        {
            return diffResult.Weight == 1;
        }
    }
}
