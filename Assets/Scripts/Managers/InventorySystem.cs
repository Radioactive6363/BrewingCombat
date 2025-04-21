using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem
{
    private Dictionary<IObject, int> inventory = new Dictionary<IObject, int>();

    // Adds item to inventory, item is an IObject
    public void AddItem(IObject item)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item]++;
        }
        else
        {
            inventory.Add(item, 1);
        }
    }

    // Removes item from inventory, item is an IObject
    public void RemoveItem(IObject item)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item]--;
            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }
        }
        else
        {
            Debug.LogWarning("The item " + item.name + " doesnt exist in the inventory.");
        }
    }

    // Removes all instances of one item from the inventory, items are IObjects
    public void CompletelyRemoveItem(IObject itemToRemove)
    {
        if (inventory.ContainsKey(itemToRemove))
        {
            inventory.Remove(itemToRemove);
        }
        else
        {
            Debug.LogWarning("The item " + itemToRemove.name + " doesnt exist in the inventory.");
        }
    }

    // Gets the count of a certain item in the inventory
    public int GetItemCount(IObject item)
    {
        if (inventory.ContainsKey(item))
        {
            return inventory[item];
        }
        else
        {
            return 0;
        }
    }

    // Returns all items of a certain ObjectType, multiple ObjectTypes can be entered at once to get the items of two or more ObjectTypes. If no objectTypes are provided, we simply get all of them.
    public Dictionary<IObject, int> GetItemsOfType(params ObjectType[] objectTypes)
    {
        Dictionary<IObject, int> itemsOfType = new Dictionary<IObject, int>();
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

        foreach (KeyValuePair<IObject, int> entry in inventory)
        {
            if (typesToFind.Contains(entry.Key.type))
            {
                if (itemsOfType.ContainsKey(entry.Key))
                {
                    itemsOfType[entry.Key] += entry.Value;
                }
                else
                {
                    itemsOfType.Add(entry.Key, entry.Value);
                }
            }
        }

        return itemsOfType;
    }
}