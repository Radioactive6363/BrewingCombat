using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private ObjectDatabaseSo objectDatabaseStart;
    [SerializeField] private bool sortAscending = false; // False = highest count first, True = lowest count first
    public static InventorySystem InstanceInventorySystem; // Singleton
    public List<IObject> Inventory;
    public UnityEvent<IObject> onInventoryChanged;
    public UnityEvent<List<IObject>> inventoryInitialized;

    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onGameRestart.AddListener(DestroyInventorySystem);
        }
        
        if (InstanceInventorySystem == null)
        {
            InstanceInventorySystem = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        Inventory = new List<IObject>();
        foreach (var item in objectDatabaseStart.objects)
        {
            if (item is IObject obj)
            {
                Inventory.Add(obj.Clone());
            }
        }
        
        // Sort inventory on initial load
        SortInventoryByCount(sortAscending);
        inventoryInitialized.Invoke(Inventory);
    }

    private void DestroyInventorySystem()
    {
        Destroy(this.gameObject);
    }

    public void OnChangedScene()
    {
        // Sort inventory when scene changes
        SortInventoryByCount(sortAscending);
        inventoryInitialized.Invoke(Inventory);
    }

    // Adds item to inventory, item is an IObject
    public void AddItem(IObject item)
    {
        if (Inventory.Contains(item))
        {
            item.Count++;
        }
        else
        {
            Inventory.Add(item.Clone());
            item.Count++;
        }
        
        // Sort inventory after adding item
        SortInventoryByCount(sortAscending);
        onInventoryChanged.Invoke(item);
    }

    // Removes item from inventory, item is an IObject
    public void RemoveItem(IObject item)
    {
        if (Inventory.Contains(item))
        {
            item.Count--;
            if (item.Count <= 0)
            {
                Inventory.Remove(item);
            }
            
            // Sort inventory after removing item
            SortInventoryByCount(sortAscending);
            onInventoryChanged.Invoke(item);
        }
        else
        {
            Debug.LogWarning("The item " + item.Name + " doesnt exist in the inventory.");
        }
    }

    // Removes all instances of one item from the inventory, items are IObjects
    private void CompletelyRemoveItem(IObject itemToRemove)
    {
        if (Inventory.Contains(itemToRemove))
        {
            itemToRemove.Count = 0;
            Inventory.Remove(itemToRemove);
            
            // Sort inventory after completely removing item
            SortInventoryByCount(sortAscending);
            onInventoryChanged.Invoke(itemToRemove);
        }
        else
        {
            Debug.LogWarning("The item " + itemToRemove.Name + " doesnt exist in the inventory.");
        }
    }

    // Returns all items of a certain ObjectType, multiple ObjectTypes can be entered at once to get the items of two or more ObjectTypes. If no objectTypes are provided, we simply get all of them.
    private List<IObject> GetItemsOfType(params ObjectType[] objectTypes)
    {
        HashSet<ObjectType> typesToFind;

        // If no objectTypes are provided, default to all possible ObjectTypes
        if (objectTypes == null || objectTypes.Length == 0)
        {
            typesToFind = new HashSet<ObjectType>((ObjectType[])Enum.GetValues(typeof(ObjectType)));
        }
        else
        {
            typesToFind = new HashSet<ObjectType>(objectTypes);
        }

        // Use LINQ to efficiently filter the inventory based on the types to find
        return Inventory.Where(item => typesToFind.Contains(item.ObjectType)).ToList();
    }

    // QUICKSORT IMPLEMENTATION
    
    // Public method to sort the inventory by count using Quicksort algorithm
    private void SortInventoryByCount(bool ascending = true)
    {
        if (Inventory.Count <= 1) return;
        
        QuickSortByCount(Inventory, 0, Inventory.Count - 1, ascending);
    }
    
    // Recursive Quicksort implementation for sorting IObjects by their Count property
    private void QuickSortByCount(List<IObject> items, int low, int high, bool ascending)
    {
        if (low < high)
        {
            // Partition the array and get the pivot index
            int pivotIndex = PartitionByCount(items, low, high, ascending);
            
            // Recursively sort elements before and after partition
            QuickSortByCount(items, low, pivotIndex - 1, ascending);
            QuickSortByCount(items, pivotIndex + 1, high, ascending);
        }
    }
    
    // Partition method for Quicksort - rearranges items around pivot based on count
    private int PartitionByCount(List<IObject> items, int low, int high, bool ascending)
    {
        int pivotCount = items[high].Count;
        
        int i = low - 1;
        
        for (int j = low; j < high; j++)
        {
            // Comparison depends on sort order
            bool shouldSwap = ascending ? 
                items[j].Count <= pivotCount : 
                items[j].Count >= pivotCount;
            
            if (shouldSwap)
            {
                i++;
                SwapItems(items, i, j);
            }
        }

        SwapItems(items, i + 1, high);
        return i + 1;
    }
    
    // Helper method to swap two items in the list
    private void SwapItems(List<IObject> items, int i, int j)
    {
        IObject temp = items[i];
        items[i] = items[j];
        items[j] = temp;
    }
}