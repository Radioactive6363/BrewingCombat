using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string Name;
    public int Health;
    public float Speed; 
    public float Damage;

    public EnemyData Clone()
    {
        return (EnemyData)MemberwiseClone();
    }
}

