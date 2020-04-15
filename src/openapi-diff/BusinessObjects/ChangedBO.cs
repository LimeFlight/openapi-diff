using openapi_diff.DTOs;

namespace openapi_diff.BusinessObjects
{
    public abstract class ChangedBO
    {
        public abstract DiffResultBO IsChanged();

        public static DiffResultBO Result(ChangedBO changed)
        {
            return changed.IsChanged() ?? new DiffResultBO(DiffResultEnum.NoChanges);
        }
        public bool IsCompatible()
        {
            return IsChanged().IsCompatible();
        }

        public bool IsIncompatible()
        {
            return IsChanged().IsIncompatible();
        }

        public bool IsUnchanged()
        {
            return IsChanged().IsUnchanged();
        }

        public bool IsDifferent()
        {
            return IsChanged().IsDifferent();
        }
    }
}
