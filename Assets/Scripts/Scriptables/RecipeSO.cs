using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevaReceta", menuName = "Items/Receta")]
public class RecipeSo : ScriptableObject
{
    [SerializeField] public string name;
    [SerializeField] public int level;
    [SerializeField] private PotionEffectType potionEffectType;
    public PotionSo result;
    
    public PotionEffectType PotionEffectType => potionEffectType;
    
    public List<IngredientSo> ingredientsByID;
    public List<IngredientSo.IngredientType> ingredientsByType;
}
