namespace VisualSorting.Sorting;

public enum Algorithm
{
    Bubble,
    Quicksort,
    Selection
}

public static class SortingExtensions
{
    private readonly static Queue<SwapAction> _actions = [];

    public static Queue<SwapAction> Sort (this List<int> list, Algorithm algorithm)
    {
        _actions.Clear();

        switch (algorithm) {

            case Algorithm.Bubble:
                BubbleSort.Sort(list, _actions);
            break;

            case Algorithm.Quicksort:
                Quicksort.Sort(list, 0, list.Count - 1, _actions);
            break;

            case Algorithm.Selection:
                SelectionSort.Sort(list, 0, _actions);
            break;
        }

        return _actions;
    }
}