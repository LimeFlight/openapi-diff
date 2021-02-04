using LimeFlight.OpenAPI.Diff.Enums;

namespace LimeFlight.OpenAPI.Diff.BusinessObjects
{
    public class ChangeBO<T>
        where T : class
    {
        private ChangeBO(T oldValue, T newValue, TypeEnum type)
        {
            OldValue = oldValue;
            NewValue = newValue;
            Type = type;
        }

        public T OldValue { get; }
        public T NewValue { get; }
        public TypeEnum Type { get; }

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