using System.Collections.Generic;
using UnityEngine;

public class CraftingController : MonoBehaviour
{
    private CraftingManager craftingManager;
    private IngredientStack _ingredientStack = new IngredientStack();
    [SerializeField] private GameObject[] _ingredientsGameObjects = new GameObject[3];
    [SerializeField] private GameObject CraftPotionButton;
    [SerializeField] private GameObject CraftingPanel;
    
    void Start()
    {
        craftingManager = FindFirstObjectByType<CraftingManager>();
        _ingredientStack.InitializeStack(_ingredientsGameObjects.Length);
    }

    public void IngredientReceived(IngredientSO ingredient)
    {
        if (!_ingredientStack.CheckFull())
        {
            _ingredientStack.StackIngredients(ingredient);
            _ingredientsGameObjects[_ingredientStack.ObtainQuantity()-1].SetActive(true);
            _ingredientsGameObjects[_ingredientStack.ObtainQuantity()-1].GetComponent<ObjectCraftingUI>().GetObjectData(ingredient);
            if (!CraftingPanel.activeSelf)
            {
                CraftingPanel.SetActive(true);
            }
            if (!CraftPotionButton.activeSelf && _ingredientStack.CheckFull())
            {
                CraftPotionButton.SetActive(true);
            }
        }
        else
        {
            FindFirstObjectByType<InventorySystem>().AddItem(ingredient);
        }
        
    }

    public void IngredientRemoved()
    { 
        if (!_ingredientStack.CheckEmptyStack())
        {
            _ingredientsGameObjects[_ingredientStack.ObtainQuantity()-1].SetActive(false);
            FindFirstObjectByType<InventorySystem>().AddItem(_ingredientStack.UnstackIngredients()); 
            if (CraftingPanel.activeSelf && _ingredientStack.CheckEmptyStack())
            {
                CraftingPanel.SetActive(false);
            }
        }
        if (CraftPotionButton.activeSelf && !_ingredientStack.CheckFull())
        {
            CraftPotionButton.SetActive(false);
        }
    }

    public void CraftPotion()
    {
        Debug.Log("Craft Potion");
        Debug.Log(_ingredientStack.ObtainQuantity());
        for (int i = 0; i < _ingredientStack.ObtainQuantity(); i++)
        {
            _ingredientsGameObjects[i].SetActive(false);
        }
        craftingManager.TryToCraftWithStack(_ingredientStack);
        CraftingPanel.SetActive(false);
        CraftPotionButton.SetActive(false);

    }
}




/*
      // Validar ingredientes
      if (botellaDeAgua == null || florRoja == null)
      {
          Debug.LogError("Error: Ingredientes no asignados en el inspector.");
          return;
      }

      // Apilar ingredientes

      // Intentar crear la poción

      //PotionSO pocion = craftingManager.TryToCraftWithStack(pilaIngredientes);

      if (pocion != null)
          Debug.Log("¡Poción creada: " + pocion.name);
      else
          Debug.Log("No se encontró ninguna receta.");
      */