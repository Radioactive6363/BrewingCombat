public interface IRecipeABBTDA
{
    RecipeSo Raiz();
    IRecipeABBTDA HijoIzq();
    IRecipeABBTDA HijoDer();
    bool ArbolVacio();
    void InicializarArbol();
    void AgregarElem(RecipeSo receta);
    void EliminarElem(int nivel);
    RecipeSo Mayor(IRecipeABBTDA nodo);
    RecipeSo Menor(IRecipeABBTDA nodo);
    RecipeSo BuscarPorNivel(int nivel);
    int CalcularAltura();
}