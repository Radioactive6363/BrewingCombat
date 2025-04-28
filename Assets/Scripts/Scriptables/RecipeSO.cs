using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevaReceta", menuName = "Items/Receta")]
public class RecipeSO : ScriptableObject
{
    [SerializeField] public string name;
    [SerializeField] public int level;
    [SerializeField] private PotionEffectType potionEffectType;
    public PotionSO result;
    
    public PotionEffectType PotionEffectType => potionEffectType;
    
    public List<IngredientSO> ingredientsByID;
    public List<IngredientSO.IngredientType> ingredientsByType;
}
