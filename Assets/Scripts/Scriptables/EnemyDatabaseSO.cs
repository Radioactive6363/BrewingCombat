using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Game/EnemyDatabase")]
public class EnemyDatabaseScriptable : ScriptableObject
{ 
    public GameObject[] enemies;
}
