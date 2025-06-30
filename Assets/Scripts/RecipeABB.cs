using UnityEngine;

public class RecipeABB : IRecipeABBTDA
{
    NodoRecipeABB raiz;

    public void InicializarArbol() => raiz = null;
    public bool ArbolVacio() => raiz == null;

    public RecipeSo Raiz() => raiz.info;
    public IRecipeABBTDA HijoIzq() => raiz.hijoIzq;
    public IRecipeABBTDA HijoDer() => raiz.hijoDer;

    public void AgregarElem(RecipeSo receta)
    {
        if (raiz == null)
        {
            raiz = new NodoRecipeABB
            {
                info = receta,
                hijoIzq = new RecipeABB(),
                hijoDer = new RecipeABB()
            };
            raiz.hijoIzq.InicializarArbol();
            raiz.hijoDer.InicializarArbol();
        }
        else if (receta.level < raiz.info.level)
            raiz.hijoIzq.AgregarElem(receta);
        else if (receta.level > raiz.info.level)
            raiz.hijoDer.AgregarElem(receta);
    }

    public void EliminarElem(int nivel)
    {
        if (raiz == null)
            return;

        if (nivel < raiz.info.level)
        {
            raiz.hijoIzq.EliminarElem(nivel);
        }
        else if (nivel > raiz.info.level)
        {
            raiz.hijoDer.EliminarElem(nivel);
        }
        else 
        {
            if (raiz.hijoIzq.ArbolVacio() && raiz.hijoDer.ArbolVacio())
            {
                raiz = null;
            }
            else if (!raiz.hijoIzq.ArbolVacio())
            {
                RecipeSo mayor = Mayor(raiz.hijoIzq);
                raiz.info = mayor;
                raiz.hijoIzq.EliminarElem(mayor.level);
            }
            else
            {
                RecipeSo menor = Menor(raiz.hijoDer);
                raiz.info = menor;
                raiz.hijoDer.EliminarElem(menor.level);
            }
        }
    }

    public RecipeSo Mayor(IRecipeABBTDA nodo)
    {
        if (nodo.HijoDer().ArbolVacio())
            return nodo.Raiz();
        return Mayor(nodo.HijoDer());
    }

    public RecipeSo Menor(IRecipeABBTDA nodo)
    {
        if (nodo.HijoIzq().ArbolVacio())
            return nodo.Raiz();
        return Menor(nodo.HijoIzq());
    }

    public RecipeSo BuscarPorNivel(int nivel)
    {
        if (raiz == null)
            return null;

        if (nivel == raiz.info.level)
            return raiz.info;
        else if (nivel < raiz.info.level)
            return raiz.hijoIzq.BuscarPorNivel(nivel);
        else
            return raiz.hijoDer.BuscarPorNivel(nivel);
    }
    public int CalcularAltura()
    {
        if (ArbolVacio())
            return -1;

        int alturaIzq = raiz.hijoIzq.CalcularAltura();
        int alturaDer = raiz.hijoDer.CalcularAltura();

        return 1 + Mathf.Max(alturaIzq, alturaDer);
    }
}