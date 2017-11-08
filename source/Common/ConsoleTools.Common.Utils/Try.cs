using System;

namespace ConsoleTools.Common.Utils
{
    public static class Try
    {
        public static void Ignore(Action action)
        {
            Ignore<Exception>(action);
        }

        public static void Ignore<TException>(Action action) where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException)
            {
                // ignore
            }
        }

        public static TResult Ignore<TResult>(Func<TResult> func) => Ignore(func, default);
        public static TResult Ignore<TResult>(Func<TResult> function, TResult defaultValue) => Ignore<Exception, TResult>(function, defaultValue);
        public static TResult Ignore<TException, TResult>(Func<TResult> func) where TException : Exception => Ignore<TException, TResult>(func, default);

        public static TResult Ignore<TException, TResult>(Func<TResult> function, TResult defaultValue) where TException : Exception
        {
            try
            {
                return function();
            }
            catch (TException)
            {
                return defaultValue;
            }
        }
    }
}
