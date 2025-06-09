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
}


