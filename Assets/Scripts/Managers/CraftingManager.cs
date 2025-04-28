using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [Header("Recetas disponibles")]
    public List<RecetaSO> todasLasRecetas;

    // Desde la pila
    public PotionSO IntentarCraftearDesdePila(PilaTI pila)
    {
        List<IngredientSO> ingredientesEnPila = new List<IngredientSO>();

        for (int i = 0; i < pila.ObtenerCantidad(); i++)
        {
            ingredientesEnPila.Add(pila.ObtenerElemento(i));
        }
        return IntentarCraftear(ingredientesEnPila);
    }

    
    public PotionSO IntentarCraftear(List<IngredientSO> ingredientesEnPila)
    {
        Debug.Log("Intentando craftear. Ingredientes en pila:");

        foreach (var ing in ingredientesEnPila)
        {
            if (ing == null)
            {
                Debug.LogError("ERROR: Hay un ingrediente NULL en la pila.");
                continue;
            }
            Debug.Log($"- {ing.name} (Tipo: {ing.Type}, ID: {ing.Id})");
        }

        foreach (var receta in todasLasRecetas)
        {
            Debug.Log("Revisando receta: " + receta.name);

            if (receta.level == 1)
            {
                if (receta.ingredientsByType == null || receta.ingredientsByType.Count == 0)
                {
                    Debug.LogError("ERROR: Receta " + receta.name + " no tiene ingredientesPorTipo configurados.");
                    continue;
                }

               
                Debug.Log("Ingredientes esperados por tipo:");
                foreach (var tipo in receta.ingredientsByType)
                {
                    Debug.Log("- " + tipo);
                }

                List<IngredientSO.IngredientType> tiposEnPila = new List<IngredientSO.IngredientType>();
                foreach (var ing in ingredientesEnPila)
                {
                    tiposEnPila.Add(ing.Type);
                }

                if (CompareByType(receta.ingredientsByType, tiposEnPila))
                {
                    Debug.Log("¡Receta encontrada! Creaste: " + receta.name);
                    return receta.result;
                }
            }
            else
            {
                if (receta.ingredientsByID == null || receta.ingredientsByID.Count == 0)
                {
                    Debug.LogError("ERROR: Receta " + receta.name + " no tiene ingredientes PorId configurados.");
                    continue;
                }

               
                Debug.Log("Ingredientes esperados por ID:");
                foreach (var ing in receta.ingredientsByID)
                {
                    Debug.Log("- " + ing.name + " (ID: " + ing.Id + ")");
                }

                if (CompareByID(receta.ingredientsByID, ingredientesEnPila))
                {
                    Debug.Log("¡Receta encontrada! Creaste: " + receta.name);
                    return receta.result;
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
