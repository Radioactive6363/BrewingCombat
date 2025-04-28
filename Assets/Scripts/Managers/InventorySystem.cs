using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] private ObjectDatabaseSO objectDatabaseStart;
    public static InventorySystem instanceInventorySystem; // Singleton
    public List<IObject> inventory;
    public UnityEvent<IObject> onInventoryChanged;
    public UnityEvent<List<IObject>> inventoryInitialized;

    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameRestart.AddListener(DestroyInventorySystem);
        }
        
        if (instanceInventorySystem == null)
        {
            instanceInventorySystem = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        inventory = new List<IObject>();
        foreach (var item in objectDatabaseStart.objects)
        {
            if (item is IObject obj)
            {
                inventory.Add(obj.Clone());
            }
        }
        inventoryInitialized.Invoke(inventory);
    }

    private void DestroyInventorySystem()
    {
        Destroy(this.gameObject);
    }

    public void OnChangedScene()
    {
        inventoryInitialized.Invoke(inventory);
    }

    // Adds item to inventory, item is an IObject
    public void AddItem(IObject item)
    {
        if (inventory.Contains(item))
        {
            item.Count++;
        }
        else
        {
            inventory.Add(item.Clone());
            item.Count++;
        }
        onInventoryChanged.Invoke(item);
    }

    // Removes item from inventory, item is an IObject
    public void RemoveItem(IObject item)
    {
        if (inventory.Contains(item))
        {
            item.Count--;
            if (item.Count <= 0)
            {
                inventory.Remove(item);
            }
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
        if (inventory.Contains(itemToRemove))
        {
            itemToRemove.Count = 0;
            inventory.Remove(itemToRemove);
            onInventoryChanged.Invoke(itemToRemove);
        }
        else
        {
            Debug.LogWarning("The item " + itemToRemove.Name + " doesnt exist in the inventory.");
        }
    }

    // Gets the count of a certain item in the inventory
    private int GetItemCount(IObject item)
    {
        if (inventory.Contains(item))
        {
            return item.Count;
        }
        else
        {
            return 0;
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
        return inventory.Where(item => typesToFind.Contains(item.ObjectType)).ToList();
    }
}