using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevaReceta", menuName = "Items/Receta")]
public class RecipeSO : ScriptableObject
{
    [SerializeField] public string name;
    [SerializeField] public int level;
    public PotionSO result;

    public List<IngredientSO> ingredientsByID;
    public List<IngredientSO.IngredientType> ingredientsByType;
}
