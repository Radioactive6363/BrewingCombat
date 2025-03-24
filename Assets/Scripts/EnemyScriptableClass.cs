using UnityEngine;

public class EnemyScriptableClass : MonoBehaviour
{
    [CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Game/EnemyDatabase")]
    public class EnemyDatabase : ScriptableObject
    {
        public GameObject[] enemies;
    }
}
