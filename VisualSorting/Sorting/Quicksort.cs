namespace VisualSorting.Sorting;

public static class Quicksort
{
    public static void Sort (this List<int> list, int low, int high, Queue<SwapAction> actions)
    {
        if (low < high) {
            int pivotIndex = Partition(list, low, high, actions);

            Sort(list, low, pivotIndex - 1, actions);
            Sort(list, pivotIndex + 1, high, actions);
        }
    }

    private static int Partition (List<int> list, int low, int high, Queue<SwapAction> actions)
    {
        int pivot = list[high];
        int i = low - 1;       

        for (int j = low; j < high; j++) {
            if (list[j] <= pivot) {
                i++;
                SwapAction.Swap(list, i, j);
                actions.Enqueue(new(i, j));
            }
        }

        SwapAction.Swap(list, i + 1, high);
        actions.Enqueue(new(i + 1, high));
        
        return i + 1;
    }
}