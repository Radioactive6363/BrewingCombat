using UnityEngine;

public interface IABBTDA
{
    IngredientSo Raiz();
    IABBTDA HijoIzq();
    IABBTDA HijoDer();
    bool ArbolVacio();
    void InicializarArbol();
    void AgregarElem(int x);
    void EliminarElem(int x);
}
