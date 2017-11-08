using ConsoleTools.Common.UserInterface.Arguments;

namespace ConsoleTools.IO.Delete.Arguments
{
    internal class RecursiveArgument : SupportedOnableArgument
    {
        #region Overrides of SupportedArgument
        public override string Key => "Recursive";
        public override string ShortKey => "R";
        public override string Description => "Denotes if deletion should be recursive. Default is false.";
        #endregion Overrides of SupportedArgument

        #region Overrides of SupportedOnableArgument
        public override bool DefaultState => false;
        #endregion Overrides of SupportedOnableArgument
    }
}