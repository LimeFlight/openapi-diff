using System.Collections.Generic;
using LimeFlight.OpenAPI.Diff.Enums;

namespace LimeFlight.OpenAPI.Diff.BusinessObjects
{
    public class ChangedMetadataBO : ChangedBO
    {
        public ChangedMetadataBO(string left, string right)
        {
            Left = left;
            Right = right;
        }

        public string Left { get; }
        public string Right { get; }

        protected override ChangedElementTypeEnum GetElementType()
        {
            return ChangedElementTypeEnum.Metadata;
        }

        public override DiffResultBO IsChanged()
        {
            return Left == Right
                ? new DiffResultBO(DiffResultEnum.NoChanges)
                : new DiffResultBO(DiffResultEnum.Metadata);
        }

        protected override List<ChangedInfoBO> GetCoreChanges()
        {
            var returnList = new List<ChangedInfoBO>();
            var elementType = GetElementType();
            const TypeEnum changeType = TypeEnum.Changed;

            if (Left != Right)
                returnList.Add(new ChangedInfoBO(elementType, changeType, "Value", Left, Right));

            return returnList;
        }
    }
}