using System.ComponentModel;

namespace LimeFlight.OpenAPI.Diff.CLI
{
    public enum ExitTypeEnum
    {
        [Description("Output diff state: no_changes, incompatible, compatible")]
        PrintState = 0,
        [Description("Fail if any API changes")]
        FailOnChanged = 1
    }
}
