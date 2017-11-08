using ConsoleTools.Common.UserInterface.Arguments;

namespace ConsoleTools.IO.Delete.Arguments
{
    internal class FilesOnlyArgument : SupportedOnableArgument
    {
        #region Overrides of SupportedArgument
        public override string Key => "FilesOnly";
        public override string ShortKey => "F";
        public override string Description => "Denotes if deletion should be for files only. Default is true.";
        #endregion Overrides of SupportedArgument

        #region Overrides of SupportedOnableArgument
        public override bool DefaultState => true;
        #endregion Overrides of SupportedOnableArgument
    }
}