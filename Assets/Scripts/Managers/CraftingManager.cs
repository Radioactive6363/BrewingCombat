using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [Header("Recetas disponibles")]
    public RecipeDatabaseSO AllRecipes;
    
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
            if (recipe is RecipeSO recipeSO)
            {
               Debug.Log("Revisando receta: " + recipeSO.name);
               
                           if (recipeSO.level == 1)
                           {
                               if (recipeSO.ingredientsByType == null || recipeSO.ingredientsByType.Count == 0)
                               {
                                   Debug.LogError("ERROR: Receta " + recipeSO.name + " no tiene ingredientes PorTipo configurados.");
                                   continue;
                               }
                               
                               Debug.Log("Ingredientes esperados por tipo:");
                               foreach (var tipo in recipeSO.ingredientsByType)
                               {
                                   Debug.Log("- " + tipo);
                               }
               
                               List<IngredientSO.IngredientType> typeOfStack = new List<IngredientSO.IngredientType>();
                               foreach (var ing in stackIngredients)
                               {
                                   typeOfStack.Add(ing.Type);
                               }
               
                               if (CompareByType(recipeSO.ingredientsByType, typeOfStack))
                               {
                                   Debug.Log("¡Receta encontrada! Creaste: " + recipeSO.name);
                                   return recipeSO.result;
                               }
                           }
                           else
                           {
                               if (recipeSO.ingredientsByID == null || recipeSO.ingredientsByID.Count == 0)
                               {
                                   Debug.LogError("ERROR: Receta " + recipeSO.name + " no tiene ingredientes PorId configurados.");
                                   continue;
                               }
                               
                               Debug.Log("Ingredientes esperados por ID:");
                               foreach (var ing in recipeSO.ingredientsByID)
                               {
                                   Debug.Log("- " + ing.name + " (ID: " + ing.Id + ")");
                               }
               
                               if (CompareByID(recipeSO.ingredientsByID, stackIngredients))
                               {
                                   Debug.Log("¡Receta encontrada! Creaste: " + recipeSO.name);
                                   return recipeSO.result;
                               }
                           } 
            }
                
        }
        Debug.Log("No se encontró ninguna receta.");
        return null;
    }

    private bool CompareByType(List<IngredientSO.IngredientType> tiposReceta, List<IngredientSO.IngredientType> tiposPila)
    {
        if (tiposReceta.Count != tiposPila.Count)
            return false;

        for (int i = 0; i < tiposReceta.Count; i++)
        {
            if (tiposReceta[i] != tiposPila[i])
                return false;
        }

        return true;
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
