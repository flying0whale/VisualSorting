namespace VisualSorting.Sorting;

public class SwapAction(int first, int second)
{
    public int First { get; } = first;
    public int Second { get; } = second;

    public static void Swap<T>(List<T> list, int first, int second)
    {
        (list[first], list[second]) = (list[second], list[first]);
    }
}