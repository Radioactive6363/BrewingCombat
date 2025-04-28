using System.Collections.Generic;
using UnityEngine;


public class IngredientsUI : MonoBehaviour
{
    private InventorySystem inventoryManager;
    private List<IObject> managerOnStartInventory;
    private Dictionary<IObject,GameObject> gameObjectInventory = new Dictionary<IObject,GameObject>();
    [SerializeField] private GameObject objectUIPrefab;
    
    private void Start()
    {
        inventoryManager = FindFirstObjectByType<InventorySystem>();
        inventoryManager.onInventoryChanged.AddListener(UpdateInventory);
        inventoryManager.inventoryInitialized.AddListener(InitializeUI);
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
                item.GetComponent<ObjectUIScript>().objectContained = obj;
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
    }
}