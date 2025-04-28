using UnityEngine;
public class PilaTI
{
    private IngredientSO[] a;
    private int cantidadMaxima;
    private int indice;

    public void InicializarPila(int cantidad)
    {
        cantidadMaxima = cantidad;
        a = new IngredientSO[cantidad];
        indice = 0;
    }

    public int Apilar(IngredientSO x)
    {
        if (indice < cantidadMaxima)
        {
            a[indice] = x;
            indice++;
            return indice;
        }
        else
        {
            Debug.LogError("Pila llena, no se puede apilar.");
            return 0;
        }
    }

    public IngredientSO Desapilar()
    {
        if (!PilaVacia())
        {
            indice--;
            IngredientSO temp = a[indice];
            a[indice] = null; // limpiar la posición
            return temp;
        }
        else
        {
            return null;
        }
    }

    public bool PilaVacia()
    {
        return indice == 0;
    }

    public IngredientSO Tope()
    {
        if (!PilaVacia())
            return a[indice - 1];
        else
            return null;
    }

    public int ObtenerCantidad()
    {
        return indice;
    }

    public IngredientSO ObtenerElemento(int index)
    {
        if (index >= 0 && index < indice)
            return a[index];
        else
            return null;
    }
}


