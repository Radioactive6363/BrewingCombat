using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/EnemyData")]
public class EnemyData : ScriptableObject
{
    [FormerlySerializedAs("Name")] public string name;
    [FormerlySerializedAs("Health")] public int health;
    [FormerlySerializedAs("Speed")] public float speed; 
    [FormerlySerializedAs("Damage")] public float damage;

    public EnemyData Clone()
    {
        return (EnemyData)MemberwiseClone();
    }
}

