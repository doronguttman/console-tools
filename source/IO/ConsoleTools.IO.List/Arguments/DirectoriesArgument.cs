namespace ConsoleTools.IO.List.Arguments
{
    internal class DirectoriesArgument : Common.UserInterface.Arguments.SupportedOnableArgument
    {
        #region Overrides of SupportedArgument
        public override string Key => "Dirs";
        public override string ShortKey => "D";
        public override string Description => "Denotes if the search should include directories, Default is true.";
        #endregion Overrides of SupportedArgument

        #region Overrides of SupportedOnableArgument
        public override bool DefaultState => true;
        #endregion Overrides of SupportedOnableArgument
    }
}