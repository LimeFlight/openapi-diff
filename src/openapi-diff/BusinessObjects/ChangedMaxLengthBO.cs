using openapi_diff.DTOs;

namespace openapi_diff.BusinessObjects
{
    public class ChangedMaxLengthBO : ChangedBO
    {
        private readonly int? _oldValue;
        private readonly int? _newValue;
        private readonly DiffContextBO _context;

        public ChangedMaxLengthBO(int oldValue, int newValue, DiffContextBO context)
        {
            _oldValue = oldValue;
            _newValue = newValue;
            _context = context;
        }

        public override DiffResultBO IsChanged()
        {
            if (_oldValue == _newValue)
            {
                return new DiffResultBO(DiffResultEnum.NoChanges);
            }
            if (_context.IsRequest && (_newValue == null || _oldValue != null && _oldValue <= _newValue)
                || _context.IsResponse && (_oldValue == null || _newValue != null && _newValue <= _oldValue))
            {
                return new DiffResultBO(DiffResultEnum.Compatible);
            }
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }
    }
}
