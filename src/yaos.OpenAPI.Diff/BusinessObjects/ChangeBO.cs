using yaos.OpenAPI.Diff.Enums;

namespace yaos.OpenAPI.Diff.BusinessObjects
{
    public class ChangeBO<T>
    where T : class
    {
        private readonly T _oldValue;
        private readonly T _newValue;
        private readonly TypeEnum _type;

        private ChangeBO(T oldValue, T newValue, TypeEnum type)
        {
            _oldValue = oldValue;
            _newValue = newValue;
            _type = type;
        }

        public static ChangeBO<T> Changed(T oldValue, T newValue)
        {
            return new ChangeBO<T>(oldValue, newValue, TypeEnum.Changed);
        }

        public static ChangeBO<T> Added(T newValue)
        {
            return new ChangeBO<T>(null, newValue, TypeEnum.Added);
        }

        public static ChangeBO<T> Removed(T oldValue)
        {
            return new ChangeBO<T>(oldValue, null, TypeEnum.Removed);
        }
    }
}
