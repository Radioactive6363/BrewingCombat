using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [Header("Recetas disponibles")]
    public RecipeDatabaseSO AllRecipes;

    public void GetPotion(IStack ingredientStack)
    {
        PotionSO potionCrafted = TryToCraftWithStack(ingredientStack);
        FindFirstObjectByType<InventorySystem>().AddItem(potionCrafted);
    }
    
    // Desde la pila
    public PotionSO TryToCraftWithStack(IStack ingredientStack)
    {
        List<IngredientSO> stackIngredients = new List<IngredientSO>();
        int tempStackQuantity = ingredientStack.ObtainQuantity();
        for (int i = 0; i < tempStackQuantity; i++)
        {
            IngredientSO ingredient = ingredientStack.UnstackIngredients();
            Debug.Log(ingredient);
            stackIngredients.Add(ingredient);
        }
        return TryToCraft(stackIngredients);
    }
    
    public PotionSO TryToCraft(List<IngredientSO> stackIngredients)
    {
        Debug.Log("Intentando craftear. Ingredientes en pila:");
        foreach (var ing in stackIngredients)
        {
            if (ing == null)
            {
                Debug.LogError("ERROR: Hay un ingrediente NULL en la pila.");
                continue;
            }
            Debug.Log($"- {ing.name} (Tipo: {ing.Type}, ID: {ing.Id})");
        }

        foreach (var recipe in AllRecipes.recipes)
        {
            if (recipe is RecipeSO recipeOS)
            {
               Debug.Log("Revisando receta: " + recipeOS.name);
               
                if (recipeOS.level == 1)
                {
                    if (recipeOS.ingredientsByType == null || recipeOS.ingredientsByType.Count == 0)
                    {
                        Debug.LogError("ERROR: Receta " + recipeOS.name + " no tiene ingredientes Por Tipo configurados.");
                                   continue;
                    }
                    Debug.Log("Ingredientes esperados por tipo:");
                    foreach (var tipo in recipeOS.ingredientsByType)
                    {
                        Debug.Log("- " + tipo);
                    }
               
                    List<IngredientSO.IngredientType> typeOfStack = new List<IngredientSO.IngredientType>();
                    foreach (var ing in stackIngredients)
                    {
                        typeOfStack.Add(ing.Type);
                    }
                    if (CompareByType(recipeOS.ingredientsByType, typeOfStack))
                    { 
                        Debug.Log("¡Receta encontrada! Creaste: " + recipeOS.name);
                        PotionSO potionCreated = recipeOS.result;
                        potionCreated.EffectType = recipeOS.PotionEffectType;
                        foreach (var ing in stackIngredients)
                        {
                            potionCreated.Potency += ing.Potency;
                            potionCreated.ChargeTime += ing.ChargeTime;
                        }
                        return potionCreated;
                    }
                }
                else
                {
                    /*
                    if (recipeOS.ingredientsByID == null || recipeOS.ingredientsByID.Count == 0)
                    {
                        Debug.LogError("ERROR: Receta " + recipeOS.name + " no tiene ingredientes PorId configurados.");
                                   continue;
                    }
                    
                    Debug.Log("Ingredientes esperados por ID:");
                    foreach (var ing in recipeOS.ingredientsByID)
                    {
                        Debug.Log("- " + ing.name + " (ID: " + ing.Id + ")");
                    }
                           
                    if (CompareByID(recipeOS.ingredientsByID, stackIngredients)) 
                    { 
                        Debug.Log("¡Receta encontrada! Creaste: " + recipeOS.name); 
                        return recipeOS.result;
                    }
                    */
                } 
            }
                
        }
        Debug.Log("No se encontró ninguna receta.");
        return null;
    }

    private bool CompareByType(List<IngredientSO.IngredientType> recipeTypes, List<IngredientSO.IngredientType> typesStack)
    {
        List<IngredientSO.IngredientType> typesRequired = new List<IngredientSO.IngredientType>(recipeTypes);
        if (recipeTypes.Count != typesStack.Count)
            return false;
        foreach (IngredientSO.IngredientType type in typesStack)
        {
            if (typesRequired.Contains(type)) 
                typesRequired.Remove(type);
        }
        if (typesRequired.Count == 0)
            return true;
        return false;
    }

    private bool CompareByID(List<IngredientSO> idRecipe, List<IngredientSO> ingStack)
    {
        if (idRecipe.Count != ingStack.Count)
            return false;

        for (int i = 0; i < idRecipe.Count; i++)
        {
            if (idRecipe[i].Id != ingStack[i].Id)
                return false;
        }

        return true;
    }
}
