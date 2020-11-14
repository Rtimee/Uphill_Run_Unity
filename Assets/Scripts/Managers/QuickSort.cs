using UnityEngine;

public class QuickSort : MonoBehaviour
{
    #region Variables

    // Singleton
    public static QuickSort Instance;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region Other Methods

    public void Sort(int[] array, GameObject[] ch,int low, int high)
    {
        int i;
        if (low < high)
        {
            i = partition(array, ch, low, high);
            Sort(array, ch, low, i - 1);
            Sort(array, ch, i + 1, high);
        }
    }
    public int partition(int[] A, GameObject[] ch, int low, int high)
    {
        int temp;
        GameObject tempCh;
        int x = A[high];
        int i = low - 1;
        for (int j = low; j <= high - 1; j++)
        {
            if (A[j] <= x)
            {
                i++;
                temp = A[i];
                tempCh = ch[i];
                A[i] = A[j];
                ch[i] = ch[j];
                A[j] = temp;
                ch[j] = tempCh; 
            }
        }
        temp = A[i + 1];
        tempCh = ch[i + 1];

        A[i + 1] = A[high];
        ch[i + 1] = ch[high];

        A[high] = temp;
        ch[high] = tempCh;

        return i + 1;
    }

    #endregion
}
