using System.Windows;

namespace VisualSorting;

public static class EventLoop
{
    public static void Run(Action action)
    {
        Task.Run(() => Application.Current.Dispatcher.Invoke(action));
    }
}