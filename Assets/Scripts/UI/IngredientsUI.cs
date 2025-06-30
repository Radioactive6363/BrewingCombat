using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientsUI : MonoBehaviour
{
    private InventorySystem _inventoryManager;
    private List<IObject> _managerOnStartInventory;
    private Dictionary<IObject, GameObject> _gameObjectInventory;
    [SerializeField] private GameObject objectUIPrefab;
    
    // Track the last known order to detect when a full refresh is needed
    private List<IObject> _lastKnownOrder;

    private void Awake()
    {
        _gameObjectInventory = new Dictionary<IObject, GameObject>();
        _lastKnownOrder = new List<IObject>();
        _inventoryManager = FindFirstObjectByType<InventorySystem>();
        _inventoryManager.inventoryInitialized.AddListener(InitializeUI);
        _inventoryManager.onInventoryChanged.AddListener(UpdateInventory);
    }

    private void InitializeUI(List<IObject> objects)
    {
        _managerOnStartInventory = objects;
        RefreshCompleteUI();
        UpdateLastKnownOrder();
    }

    private void RefreshCompleteUI()
    {
        // Clear existing UI elements
        ClearAllUIElements();
        
        // Use the current inventory state, not just the initial inventory
        List<IObject> currentInventory = _inventoryManager.Inventory;
        
        // Recreate UI elements in the correct sorted order
        foreach (IObject obj in currentInventory)
        {
            if (obj.Count > 0)
            {
                GameObject item = Instantiate(objectUIPrefab, transform);
                item.GetComponent<ObjectUIScript>().ObjectContained = obj;
                _gameObjectInventory.Add(obj, item);
            }
        }
    }

    private void ClearAllUIElements()
    {
        // Destroy all existing UI GameObjects
        foreach (var kvp in _gameObjectInventory)
        {
            if (kvp.Value != null)
            {
                Destroy(kvp.Value);
            }
        }
        
        // Clear the dictionary
        _gameObjectInventory.Clear();
    }

    private void UpdateInventory(IObject objectToUpdate)
    {
        // Check if the order has changed (indicating a sort happened)
        if (HasOrderChanged())
        {
            RefreshCompleteUI();
            UpdateLastKnownOrder();
        }
        else
        {
            if (_gameObjectInventory.TryGetValue(objectToUpdate, out GameObject gameObject))
            {
                if (objectToUpdate.Count <= 0)
                {
                    _gameObjectInventory.Remove(objectToUpdate);
                    Destroy(gameObject);
                }
                else
                {
                    gameObject.GetComponent<ObjectUIScript>().UpdateQuantity();
                }
            }
            else if (objectToUpdate.Count > 0)
            {
                GameObject item = Instantiate(objectUIPrefab, transform);
                item.GetComponent<ObjectUIScript>().ObjectContained = objectToUpdate;
                _gameObjectInventory.Add(objectToUpdate, item);
            }
            
            UpdateLastKnownOrder();
        }
    }

    private bool HasOrderChanged()
    {
        // Get current visible items (items with count > 0)
        var currentVisibleItems = _inventoryManager.Inventory.Where(item => item.Count > 0).ToList();
        var lastVisibleItems = _lastKnownOrder.Where(item => item.Count > 0).ToList();
        
        // Check if the order or items have changed
        if (currentVisibleItems.Count != lastVisibleItems.Count)
            return true;
            
        for (int i = 0; i < currentVisibleItems.Count; i++)
        {
            if (currentVisibleItems[i] != lastVisibleItems[i])
                return true;
        }
        
        return false;
    }

    private void UpdateLastKnownOrder()
    {
        _lastKnownOrder.Clear();
        _lastKnownOrder.AddRange(_inventoryManager.Inventory);
    }
}