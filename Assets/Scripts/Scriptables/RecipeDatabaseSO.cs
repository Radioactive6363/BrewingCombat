using UnityEngine;

[CreateAssetMenu(fileName = "RecipeDatabase", menuName = "Game/RecipeDatabase")]
public class RecipeDatabaseSO : ScriptableObject
{ 
    public ScriptableObject[] recipes;
}
