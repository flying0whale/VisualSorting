namespace VisualSorting.Sorting;

public static class SelectionSort
{
    public static void Sort (this List<int> list, int start, Queue<SwapAction> actions)
    {
        if (start == list.Count) {
            return;
        }

        int minIndex = start;

        for (int i = start; i < list.Count; i++) {
            if (list[i] < list[minIndex]) {
                minIndex = i;
            }
        }

        SwapAction.Swap(list, minIndex, start);
        actions.Enqueue(new(minIndex, start));

        Sort(list, ++start, actions);
    }
}