using System.Collections.Generic;
using LimeFlight.OpenAPI.Diff.Enums;

namespace LimeFlight.OpenAPI.Diff.BusinessObjects
{
    public class ChangedMaxLengthBO : ChangedBO
    {
        private readonly DiffContextBO _context;
        private readonly int? _newValue;

        private readonly int? _oldValue;

        public ChangedMaxLengthBO(int? oldValue, int? newValue, DiffContextBO context)
        {
            _oldValue = oldValue;
            _newValue = newValue;
            _context = context;
        }

        protected override ChangedElementTypeEnum GetElementType()
        {
            return ChangedElementTypeEnum.MaxLength;
        }

        public override DiffResultBO IsChanged()
        {
            if (_oldValue == _newValue) return new DiffResultBO(DiffResultEnum.NoChanges);
            if (_context.IsRequest && (_newValue == null || _oldValue != null && _oldValue <= _newValue)
                || _context.IsResponse && (_oldValue == null || _newValue != null && _newValue <= _oldValue))
                return new DiffResultBO(DiffResultEnum.Compatible);
            return new DiffResultBO(DiffResultEnum.Incompatible);
        }

        protected override List<ChangedInfoBO> GetCoreChanges()
        {
            var returnList = new List<ChangedInfoBO>();
            var elementType = GetElementType();
            const TypeEnum changeType = TypeEnum.Changed;

            if (_oldValue != _newValue)
                returnList.Add(new ChangedInfoBO(elementType, changeType, _context.GetDiffContextElementType(),
                    _oldValue?.ToString(), _newValue?.ToString()));

            return returnList;
        }
    }
}