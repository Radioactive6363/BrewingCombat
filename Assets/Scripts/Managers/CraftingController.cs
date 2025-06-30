using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CraftingController : MonoBehaviour
{
    private CraftingManager _craftingManager;
    private IngredientStack _ingredientStack = new IngredientStack();
    [FormerlySerializedAs("_ingredientsGameObjects")] [SerializeField] private GameObject[] ingredientsGameObjects = new GameObject[3];
    [FormerlySerializedAs("CraftPotionButton")] [SerializeField] private GameObject craftPotionButton;
    [FormerlySerializedAs("CraftingPanel")] [SerializeField] private GameObject craftingPanel;
    
    void Start()
    {
        _craftingManager = FindFirstObjectByType<CraftingManager>();
        _ingredientStack.InitializeStack(ingredientsGameObjects.Length);
    }

    public void IngredientReceived(IngredientSo ingredient)
    {
        if (!_ingredientStack.CheckFull())
        {
            _ingredientStack.StackIngredients(ingredient);
            ingredientsGameObjects[_ingredientStack.ObtainQuantity()-1].SetActive(true);
            ingredientsGameObjects[_ingredientStack.ObtainQuantity()-1].GetComponent<ObjectCraftingUI>().GetObjectData(ingredient);
            if (!craftingPanel.activeSelf)
            {
                craftingPanel.SetActive(true);
            }
            if (!craftPotionButton.activeSelf && _ingredientStack.CheckFull())
            {
                craftPotionButton.SetActive(true);
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
            ingredientsGameObjects[_ingredientStack.ObtainQuantity()-1].SetActive(false);
            FindFirstObjectByType<InventorySystem>().AddItem(_ingredientStack.UnstackIngredients()); 
            if (craftingPanel.activeSelf && _ingredientStack.CheckEmptyStack())
            {
                craftingPanel.SetActive(false);
            }
        }
        if (craftPotionButton.activeSelf && !_ingredientStack.CheckFull())
        {
            craftPotionButton.SetActive(false);
        }
    }

    public void CraftPotion()
    {
        if (_craftingManager.TryGetPotion(_ingredientStack))
        {
            Debug.Log("Craft Potion");
            Debug.Log(_ingredientStack.ObtainQuantity());
            for (int i = 0; i < _ingredientStack.ObtainQuantity(); i++)
            {
                ingredientsGameObjects[i].SetActive(false);
            }
            craftingPanel.SetActive(false);
            craftPotionButton.SetActive(false);
        }
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