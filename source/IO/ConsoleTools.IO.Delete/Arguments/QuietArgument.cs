using ConsoleTools.Common.UserInterface.Arguments;

namespace ConsoleTools.IO.Delete.Arguments
{
    internal class QuietArgument : SupportedOnableArgument
    {
        #region Overrides of SupportedArgument
        public override string Key => "Quiet";
        public override string ShortKey => "Q";
        public override string Description => "Denotes if deletion should not ask for confirmation. Default is false.";
        #endregion Overrides of SupportedArgument

        #region Overrides of SupportedOnableArgument
        public override bool DefaultState => false;
        #endregion Overrides of SupportedOnableArgument
    }
}
