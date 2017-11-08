namespace ConsoleTools.IO.List.Arguments
{
    internal class RecursiveArgument : Common.UserInterface.Arguments.SupportedOnableArgument
    {
        #region Overrides of SupportedArgument
        public override string Key => "Recursive";
        public override string ShortKey => "R";
        public override string Description => "Denotes if the search should be recursive, Default is true.";
        #endregion Overrides of SupportedArgument

        #region Overrides of SupportedOnableArgument
        public override bool DefaultState => true;
        #endregion Overrides of SupportedOnableArgument
    }
}
