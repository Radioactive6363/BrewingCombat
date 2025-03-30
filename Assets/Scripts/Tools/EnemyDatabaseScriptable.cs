using UnityEngine;

public class EnemyDatabaseScriptable : MonoBehaviour
{
    [CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Game/EnemyDatabase")]
    public class EnemyDatabase : ScriptableObject
    {
        public GameObject[] enemies;
    }
}
