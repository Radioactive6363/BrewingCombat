public interface IStack
{
    void InitializeStack(int maxQuantity);
    int StackIngredients(IngredientSo x);
    IngredientSo UnstackIngredients();
    bool CheckEmptyStack();
    int ObtainQuantity();
    bool CheckFull();
}
