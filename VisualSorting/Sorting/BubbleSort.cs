namespace VisualSorting.Sorting;

public static class BubbleSort
{
    public static void Sort (List<int> list, Queue<SwapAction> actions)
    {
        int n = list.Count;
        for (int i = 0; i < n - 1; i++) {
            for (int j = 0; j < n - i - 1; j++) {
                if (list[j] > list[j + 1]) {
                    SwapAction.Swap(list, j, j + 1);
                    actions.Enqueue(new(j, j + 1));
                }
            }
        }
    }
}