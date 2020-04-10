using openapi_diff.DTOs;

namespace openapi_diff.BusinessObjects
{
    public static class ChangedBO
    {
        static DiffResultEnum Result(this ChangedDTO changed)
        {
            return changed?.IsChanged.Status ?? DiffResultEnum.NoChanges;
        }
        static bool IsCompatible(this ChangedDTO changed)
        {
            return changed.IsChanged.IsCompatible();
        }

        static bool IsIncompatible(this ChangedDTO changed)
        {
            return changed.IsChanged.IsIncompatible();
        }

        static bool IsUnchanged(this ChangedDTO changed)
        {
            return changed.IsChanged.IsUnchanged();
        }

        static bool IsDifferent(this ChangedDTO changed)
        {
            return changed.IsChanged.IsDifferent();
        }
    }
}
