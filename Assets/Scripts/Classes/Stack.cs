using UnityEngine;

public class Stack : IStack
{
    public int[] a { get; set; }
    public int max_quantity_data { get; set; }
    public int index { get; set; }

    public void InitializeStack(int quantity)
    {
        max_quantity_data = quantity;
        a = new int[quantity];
        index = 0;
    }

    public int Stacking(int x)
    {
        if (index < max_quantity_data)
        {
            a[index] = x;
            index++;
            return index;
        }
        else
        {
            return 0;
        }

    }
    public int Unstack()
    {
        if (!EmptyStack())
        {
            index--;
            return index;
        }
        else
        {
            return 0;
        }

    }

    public bool EmptyStack()
    {
        return (index == 0);
    }

    public int Top()
    {
        return a[index - 1];
    }
    
}
