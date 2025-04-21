using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySystem
{
    private List<IObject> inventory = new List<IObject>();

    // Adds item to inventory, item is an IObject
    public void AddItem(IObject item)
    {
        if (inventory.Contains(item))
        {
            item.count++;
        }
        else
        {
            inventory.Add(item);
            item.count++;
        }
    }

    // Removes item from inventory, item is an IObject
    public void RemoveItem(IObject item)
    {
        if (inventory.Contains(item))
        {
            item.count--;
            if (item.count <= 0)
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
        if (inventory.Contains(itemToRemove))
        {
            itemToRemove.count = 0;
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
        if (inventory.Contains(item))
        {
            return item.count;
        }
        else
        {
            return 0;
        }
    }

    // Returns all items of a certain ObjectType, multiple ObjectTypes can be entered at once to get the items of two or more ObjectTypes. If no objectTypes are provided, we simply get all of them.
    public List<IObject> GetItemsOfType(params ObjectType[] objectTypes)
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
        return inventory.Where(item => typesToFind.Contains(item.type)).ToList();
    }
}