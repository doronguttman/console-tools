namespace ConsoleTools.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static T OrDefault<T>(this T obj, T defaultValue = default) where T : class => obj ?? defaultValue;
    }
}
