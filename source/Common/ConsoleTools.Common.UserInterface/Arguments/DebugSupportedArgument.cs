namespace ConsoleTools.Common.UserInterface.Arguments
{
    public class DebugSupportedArgument : SupportedOnableArgument
    {
        #region Overrides of SupportedArgument
        public override string Key => "Debug";
        public override string Description => "Denotes debug mode and will break the program upon entry. Default is false.";
        #endregion Overrides of SupportedArgument

        #region Overrides of SupportedOnableArgument
        public override bool DefaultState => false;
        #endregion Overrides of SupportedOnableArgument
    }
}