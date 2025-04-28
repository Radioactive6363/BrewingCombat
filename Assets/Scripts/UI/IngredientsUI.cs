using System;
using System.Collections.Generic;
using UnityEngine;


public class IngredientsUI : MonoBehaviour
{
    private InventorySystem inventoryManager;
    private List<IObject> managerOnStartInventory;
    private Dictionary<IObject, GameObject> gameObjectInventory;
    [SerializeField] private GameObject objectUIPrefab;

    private void Start()
    {
        gameObjectInventory = new Dictionary<IObject, GameObject>();
        inventoryManager = FindFirstObjectByType<InventorySystem>();
        inventoryManager.inventoryInitialized.AddListener(InitializeUI);
        inventoryManager.onInventoryChanged.AddListener(UpdateInventory);
    }

    private void InitializeUI(List<IObject> objects)
    {
        managerOnStartInventory = objects;
        UpdateInventoryOnStartUI();
    }

    private void UpdateInventoryOnStartUI()
    {
        foreach (IObject obj in managerOnStartInventory)
        {
            if (obj.Count > 0)
            {
                GameObject item = Instantiate(objectUIPrefab, transform);
                item.GetComponent<ObjectUIScript>().ObjectContained = obj;
                gameObjectInventory.Add(obj, item);
            }
        }
    }

    private void UpdateInventory(IObject objectToUpdate)
    {
        if (gameObjectInventory.TryGetValue(objectToUpdate, out GameObject gameObject))
        {
            if (objectToUpdate.Count <= 0)
            {
                gameObjectInventory.Remove(objectToUpdate);
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
            gameObjectInventory.Add(objectToUpdate, item);
            
        }
    }
}