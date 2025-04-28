using UnityEngine;

public class CraftingController : MonoBehaviour
{
    public CraftingManager craftingManager;

    [Header("Ingredientes disponibles para apilar")]
    public IngredientSO botellaDeAgua;
    public IngredientSO florRoja;

    private PilaTI pilaIngredientes;

    void Start()
    {
        craftingManager = FindFirstObjectByType<CraftingManager>();
        pilaIngredientes = new PilaTI();
        pilaIngredientes.InicializarPila(10);

        // Validar ingredientes
        if (botellaDeAgua == null || florRoja == null)
        {
            Debug.LogError("Error: Ingredientes no asignados en el inspector.");
            return;
        }

        // Apilar ingredientes
        pilaIngredientes.Apilar(botellaDeAgua);
        pilaIngredientes.Apilar(florRoja);
        pilaIngredientes.Apilar(florRoja);

        // Intentar crear la poción
        PotionSO pocion = craftingManager.IntentarCraftearDesdePila(pilaIngredientes);

        if (pocion != null)
            Debug.Log("¡Poción creada: " + pocion.name);
        else
            Debug.Log("No se encontró ninguna receta.");
    }
}

