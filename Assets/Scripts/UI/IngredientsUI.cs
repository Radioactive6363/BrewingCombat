using System;
using System.Collections.Generic;
using UnityEngine;


public class IngredientsUI : MonoBehaviour
{
    private InventorySystem _inventoryManager;
    private List<IObject> _managerOnStartInventory;
    private Dictionary<IObject, GameObject> _gameObjectInventory;
    [SerializeField] private GameObject objectUIPrefab;

    private void Awake()
    {
        _gameObjectInventory = new Dictionary<IObject, GameObject>();
        _inventoryManager = FindFirstObjectByType<InventorySystem>();
        _inventoryManager.inventoryInitialized.AddListener(InitializeUI);
        _inventoryManager.onInventoryChanged.AddListener(UpdateInventory);
    }

    private void InitializeUI(List<IObject> objects)
    {
        _managerOnStartInventory = objects;
        UpdateInventoryOnStartUI();
    }

    private void UpdateInventoryOnStartUI()
    {
        foreach (IObject obj in _managerOnStartInventory)
        {
            if (obj.Count > 0)
            {
                GameObject item = Instantiate(objectUIPrefab, transform);
                item.GetComponent<ObjectUIScript>().ObjectContained = obj;
                _gameObjectInventory.Add(obj, item);
            }
        }
    }

    private void UpdateInventory(IObject objectToUpdate)
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
        else
        {
            GameObject item = Instantiate(objectUIPrefab, transform);
            item.GetComponent<ObjectUIScript>().ObjectContained = objectToUpdate;
            _gameObjectInventory.Add(objectToUpdate, item);
            
        }
    }
}