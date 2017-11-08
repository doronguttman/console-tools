using ConsoleTools.Common.UserInterface.Arguments;

namespace ConsoleTools.IO.Delete.Arguments
{
    internal class RemoveEmptyDirsArgument : SupportedOnableArgument
    {
        #region Overrides of SupportedArgument
        public override string Key => "RemoveEmptyDirs";
        public override string ShortKey => "E";
        public override string Description => "When used together with \"+FilesOnly\", if set, will delete empty directories. Default is false.";
        #endregion Overrides of SupportedArgument

        #region Overrides of SupportedOnableArgument
        public override bool DefaultState => false;
        #endregion Overrides of SupportedOnableArgument
    }
}