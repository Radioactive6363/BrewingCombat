using UnityEngine;

public class IngredientStack : IStack
{
    private IngredientSO[] a;
    private int maxQuantity;
    private int index;

    public void InitializeStack(int Quantity)
    {
        maxQuantity = Quantity;
        a = new IngredientSO[maxQuantity];
        index = 0;
    }

    public int StackIngredients(IngredientSO x)
    {
        if (index < maxQuantity)
        {
            a[index] = x;
            index++;
            return index;
        }
        else
        {
            Debug.LogError("Pila llena, no se puede apilar.");
            return 0;
        }
    }

    public IngredientSO UnstackIngredients()
    {
        if (!CheckEmptyStack())
        {
            index--;
            IngredientSO temp = a[index];
            a[index] = null; // limpiar la posición
            return temp;
        }
        else
        {
            return null;
        }
    }

    public bool CheckEmptyStack()
    {
        return index == 0;
    }
    

    public int ObtainQuantity()
    {
        return index;
    }
    
    public bool CheckFull()
    {
        return index == maxQuantity;
    }
}


