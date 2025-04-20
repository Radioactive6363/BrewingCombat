using UnityEngine;

public interface IStack
{
    int[] a { get; set; }
    int max_quantity_data { get; set; }
    int index { get; set; }

    public void InitializeStack(int quantity);
    public int Stacking(int x);
    public int Unstack();
    public bool EmptyStack();
    public int Top();

}
