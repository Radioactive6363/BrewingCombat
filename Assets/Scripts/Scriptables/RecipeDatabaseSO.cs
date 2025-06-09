using UnityEngine;

[CreateAssetMenu(fileName = "RecipeDatabase", menuName = "Game/RecipeDatabase")]
public class RecipeDatabaseSo : ScriptableObject
{ 
    public ScriptableObject[] recipes;
}
