using UnityEngine;

public class EnemyScriptable : MonoBehaviour
{
    [CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Game/EnemyDatabase")]
    public class EnemyData : ScriptableObject
    {
        public string Name;
        public int Health;
        public float Speed; 
        public float Damage;
        public Mesh enemyMesh;
    }
}
