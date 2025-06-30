using Unity.VisualScripting;
using UnityEngine;

public class IngredientStack : IStack
{
    private IngredientSo[] _a;
    private int _maxQuantity;
    private int _index;

    public void InitializeStack(int quantity)
    {
        _maxQuantity = quantity;
        _a = new IngredientSo[_maxQuantity];
        _index = 0;
    }

    public int StackIngredients(IngredientSo x)
    {
        if (_index < _maxQuantity)
        {
            _a[_index] = x;
            _index++;
            return _index;
        }
        else
        {
            Debug.LogError("Pila llena, no se puede apilar.");
            return 0;
        }
    }

    public IngredientSo UnstackIngredients()
    {
        if (!CheckEmptyStack())
        {
            _index--;
            IngredientSo temp = _a[_index];
            _a[_index] = null; // limpiar la posición
            return temp;
        }
        else
        {
            return null;
        }
    }

    public bool CheckEmptyStack()
    {
        return _index == 0;
    }
    

    public int ObtainQuantity()
    {
        return _index;
    }
    
    public bool CheckFull()
    {
        return _index == _maxQuantity;
    }

    public IStack Clone()
    {
        // Create a new IngredientStack with the same capacity
        IngredientStack clonedStack = new IngredientStack();
        clonedStack.InitializeStack(_maxQuantity);
        
        // Copy all ingredients from the current stack to the cloned stack
        for (int i = 0; i < _index; i++)
        {
            clonedStack._a[i] = _a[i];
        }
        
        // Set the same index position
        clonedStack._index = _index;
        
        return clonedStack;
    }
}


