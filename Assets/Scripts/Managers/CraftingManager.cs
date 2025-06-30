using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CraftingManager : MonoBehaviour
{
    [Header("Recetas disponibles")]
    private Dictionary<PotionEffectType, RecipeABB> diccionarioDeArboles;
    public RecipeDatabaseSo allRecipes;

    private void Awake()
    {
        diccionarioDeArboles = new Dictionary<PotionEffectType, RecipeABB>();
        foreach (var receta in allRecipes.recipes)
        {
            if (receta is RecipeSo recetaSo)
            {
                if (!diccionarioDeArboles.TryGetValue(recetaSo.PotionEffectType, out RecipeABB arbol))
                {
                    arbol = new RecipeABB();
                    arbol.InicializarArbol();
                    diccionarioDeArboles[recetaSo.PotionEffectType] = arbol;
                }
                arbol.AgregarElem(recetaSo);
            }
        }
        MostrarRecetasOrdenadas(); //Debugging
    }
    
    public void MostrarRecetasOrdenadas()
    {
        Debug.Log("Arbol de recetas (ordenado por nivel):");
        foreach (var keyValue in diccionarioDeArboles)
        {
            Debug.Log($"--- Arbol para efecto: {keyValue.Key} ---");
            MostrarRecetasRecursivo(keyValue.Value);
        }
    }

    private void MostrarRecetasRecursivo(IRecipeABBTDA nodo)
    {
        if (!nodo.ArbolVacio())
        {
            MostrarRecetasRecursivo(nodo.HijoIzq());

            string nombre = nodo.Raiz().name;
            int nivel = nodo.Raiz().level;
            int altura = nodo.CalcularAltura();

            string hijoIzq = nodo.HijoIzq().ArbolVacio() ? "null" : nodo.HijoIzq().Raiz().name + $" (Nivel {nodo.HijoIzq().Raiz().level})";
            string hijoDer = nodo.HijoDer().ArbolVacio() ? "null" : nodo.HijoDer().Raiz().name + $" (Nivel {nodo.HijoDer().Raiz().level})";

            Debug.Log($"Nodo actual -> Nombre: {nombre}, Nivel: {nivel}, Altura: {altura}\n" +
                      $"   Hijo Izquierdo: {hijoIzq}\n" +
                      $"   Hijo Derecho: {hijoDer}");

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
               
                if (recipeOS.ingredientsByType == null || recipeOS.ingredientsByType.Count == 0)
                {
                    Debug.LogError(
                        "ERROR: Receta " + recipeOS.name + " no tiene ingredientes Por Tipo configurados.");
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
                    if (diccionarioDeArboles.TryGetValue(recipeOS.PotionEffectType, out var arbolDeMejoras))
                    {
                        RecipeSo recetaMejorada = arbolDeMejoras.BuscarPorNivel(recipeOS.level);
                        if (recetaMejorada != null && CompareByID(recetaMejorada.ingredientsByID, stackIngredients))
                        {
                            potionCreated = recetaMejorada.result;
                        }
                        potionCreated.EffectType = recipeOS.PotionEffectType;
                        foreach (var ing in stackIngredients)
                        {
                            int appliedPotency = ing.Potency * recipeOS.level; 
                            potionCreated.Potency += appliedPotency;
                            potionCreated.ChargeTime += ing.ChargeTime;
                        }
                        Debug.Log($"¡Mejora encontrada! Creaste: {recetaMejorada.name}\n" +
                                  $"Potency: {potionCreated?.Potency}\n" +
                                  $"Level: {recipeOS?.level}");
                        return potionCreated;
                    } 
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