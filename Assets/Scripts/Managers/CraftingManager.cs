using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CraftingManager : MonoBehaviour
{
    [Header("Recetas disponibles")]
    private RecipeABB arbolDeMejoras;
    public RecipeDatabaseSo allRecipes;

    private void Awake()
    {
        arbolDeMejoras = new RecipeABB();
        arbolDeMejoras.InicializarArbol();

        foreach (var receta in allRecipes.recipes)
        {
            if (receta is RecipeSo recetaSo)
            {
                arbolDeMejoras.AgregarElem(recetaSo);
            }
        }
        MostrarRecetasOrdenadas(); //Debugging
    }
    
    public void MostrarRecetasOrdenadas()
    {
        Debug.Log("Árbol de recetas (ordenado por nivel):");
        MostrarRecetasRecursivo(arbolDeMejoras);
    }

    private void MostrarRecetasRecursivo(IRecipeABBTDA nodo)
    {
        if (!nodo.ArbolVacio())
        {
            MostrarRecetasRecursivo(nodo.HijoIzq());
            Debug.Log($"Nivel: {nodo.Raiz().level}, Nombre: {nodo.Raiz().name}");
            MostrarRecetasRecursivo(nodo.HijoDer());
        }
    }
    
    public bool TryGetPotion(IStack ingredientStack)
    {
        PotionSo potionCrafted = TryToCraftWithStack(ingredientStack);

        if (potionCrafted != null)
        {
            FindFirstObjectByType<InventorySystem>().AddItem(potionCrafted);
            return true;
        }

        return false;
    }
    
    // Desde la pila
    public PotionSo TryToCraftWithStack(IStack ingredientStack)
    {
        List<IngredientSo> stackIngredients = new List<IngredientSo>();
        IStack tempStack = ingredientStack.Clone();
        int tempStackQuantity = tempStack.ObtainQuantity();
        for (int i = 0; i < tempStackQuantity; i++)
        {
            IngredientSo ingredient = tempStack.UnstackIngredients();
            Debug.Log(ingredient);
            stackIngredients.Add(ingredient);
        }
        return TryToCraft(stackIngredients);
    }
    
    public PotionSo TryToCraft(List<IngredientSo> stackIngredients)
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

        foreach (var recipe in allRecipes.recipes)
        {
            if (recipe is RecipeSo recipeOS)
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
               
                    List<IngredientSo.IngredientType> typeOfStack = new List<IngredientSo.IngredientType>();
                    foreach (var ing in stackIngredients)
                    {
                        typeOfStack.Add(ing.Type);
                    }
                    if (CompareByType(recipeOS.ingredientsByType, typeOfStack))
                    { 
                        Debug.Log("¡Receta encontrada! Creaste: " + recipeOS.name);
                        PotionSo potionCreated = recipeOS.result;
                        //Mejora de pocion
                        RecipeSo recetaMejorada = arbolDeMejoras.BuscarPorNivel(recipeOS.level + 1);
                        if (recetaMejorada != null && CompareByID(recetaMejorada.ingredientsByID, stackIngredients))
                        {
                            Debug.Log("¡Mejora encontrada! Creaste: " + recetaMejorada.name);
                            potionCreated = recetaMejorada.result;
                        }
                        
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

    private bool CompareByType(List<IngredientSo.IngredientType> recipeTypes, List<IngredientSo.IngredientType> typesStack)
    {
        List<IngredientSo.IngredientType> typesRequired = new List<IngredientSo.IngredientType>(recipeTypes);
        if (recipeTypes.Count != typesStack.Count)
            return false;
        foreach (IngredientSo.IngredientType type in typesStack)
        {
            if (typesRequired.Contains(type)) 
                typesRequired.Remove(type);
        }
        if (typesRequired.Count == 0)
            return true;
        return false;
    }

    private bool CompareByID(List<IngredientSo> idRecipe, List<IngredientSo> ingStack)
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
