using UnityEngine;
using System;
using System.Collections.Generic;
public class PotionHistorySystem : MonoBehaviour
{
    public static PotionHistorySystem Instance { get; private set; }

    public List<PotionHistoryEntry> potionHistory = new List<PotionHistoryEntry>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddToHistory(PotionSo potion)
    {
        potionHistory.Add(new PotionHistoryEntry(potion));
    }

    public void SortHistoryByDate(bool newestFirst = true)
    {
        if (potionHistory.Count <= 1) return;
        QuickSortByDate(potionHistory, 0, potionHistory.Count - 1, newestFirst);
    }

    private void QuickSortByDate(List<PotionHistoryEntry> list, int low, int high, bool newestFirst)
    {
        if (low < high)
        {
            int pivotIndex = Partition(list, low, high, newestFirst);
            QuickSortByDate(list, low, pivotIndex - 1, newestFirst);
            QuickSortByDate(list, pivotIndex + 1, high, newestFirst);
        }
    }

    private int Partition(List<PotionHistoryEntry> list, int low, int high, bool newestFirst)
    {
        DateTime pivotDate = list[high].createdAt;
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            bool shouldSwap = newestFirst ?
                list[j].createdAt >= pivotDate :
                list[j].createdAt <= pivotDate;

            if (shouldSwap)
            {
                i++;
                Swap(list, i, j);
            }
        }

        Swap(list, i + 1, high);
        return i + 1;
    }

    private void Swap(List<PotionHistoryEntry> list, int i, int j)
    {
        PotionHistoryEntry temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }


    public void PrintHistoryToConsole()
    {
        Debug.Log(" Potion Crafting History:");

        if (potionHistory.Count == 0)
        {
            Debug.Log(" No potions crafted yet.");
            return;
        }

        foreach (var entry in potionHistory)
        {
            Debug.Log($" {entry.potion.Name} crafted at {entry.createdAt}");
        }
    }
}
