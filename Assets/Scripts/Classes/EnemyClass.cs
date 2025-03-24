using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    protected int Health;
    protected float Speed;
    protected float Damage;
    protected List <AbilityStruct> Abilities = new List<AbilityStruct>();
    
    public AbilityStruct GetRandomAbility()
    {
        float roll = Random.value; //0-1
        float probabilityNumber = 0f;
        
        foreach (var AbilityStruct in Abilities)
        {
            probabilityNumber += AbilityStruct.probability;
            if (roll <= probabilityNumber)
            {
                return AbilityStruct;
            }
        }
        return Abilities[0];
    }
}

