namespace ConsoleTools.IO.List.Arguments
{
    internal class FilesArgument : Common.UserInterface.Arguments.SupportedOnableArgument
    {
        #region Overrides of SupportedArgument
        public override string Key => "Files";
        public override string ShortKey => "F";
        public override string Description => "Denotes if the search should include files, Default is true.";
        #endregion Overrides of SupportedArgument

        #region Overrides of SupportedOnableArgument
        public override bool DefaultState => true;
        #endregion Overrides of SupportedOnableArgument
    }
}