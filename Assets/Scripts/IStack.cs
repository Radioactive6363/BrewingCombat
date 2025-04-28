public interface IStack
{
    void InitializeStack(int maxQuantity);
    int StackIngredients(IngredientSO x);
    IngredientSO UnstackIngredients();
    bool CheckEmptyStack();
    int ObtainQuantity();
    bool CheckFull();
}
