using openapi_diff.DTOs;

namespace openapi_diff.BusinessObjects
{
    public class ChangedWriteOnlyBO : ChangedBO
    {
        private readonly DiffContextBO _context;
        private readonly bool _oldValue;
        private readonly bool _newValue;

        public ChangedWriteOnlyBO(bool? oldValue, bool? newValue, DiffContextBO context)
        {
            _context = context;
            _oldValue = oldValue ?? false;
            _newValue = newValue ?? false;
        }

        public override DiffResultBO IsChanged()
        {
            if (_oldValue == _newValue)
            {
                return new DiffResultBO(DiffResultEnum.NoChanges);
            }
            if (_context.IsRequest)
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            if (_context.IsResponse)
            {
                if (_newValue)
                {
                    return new DiffResultBO(DiffResultEnum.Incompatible);
                }

                return _context.IsRequired ? new DiffResultBO(DiffResultEnum.Incompatible) : new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Unknown);
        }
    }
}
