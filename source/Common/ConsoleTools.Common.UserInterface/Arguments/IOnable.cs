namespace ConsoleTools.Common.UserInterface.Arguments
{
    public interface IOnable
    {
        bool DefaultState { get; }
        bool IsOn(ArgumentParser argumentParser);
    }
}
