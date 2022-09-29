namespace Slight.Core
{
    public static class DispatcherExtensions
    {
        public static void Invoke(this IDispatcher dispatcher, Action action)
        {
            dispatcher.Dispatch(action);
        }

        public static TReturn Invoke<TReturn>(this IDispatcher dispatcher, Func<TReturn> func)
        {
            TReturn @default = default(TReturn);

            dispatcher.Dispatch(() => @default = func());

            return @default;
        }


        public static void InvokeAsync(this IDispatcher dispatcher, Action invokeCommand)
        {
            dispatcher.DispatchDelayed(TimeSpan.Zero, invokeCommand);
        }
    }
}
