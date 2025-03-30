using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyClass : MonoBehaviour
{
    protected EnemyScriptable.EnemyData enemyData;
    [SerializeField] List <AbilityStruct> Abilities = new List<AbilityStruct>();
    private int currentHealth;
    
    private void Start()
    {
        //References Enemy Data
        if (enemyData != null)
        {
            gameObject.name = enemyData.Name;
            currentHealth = enemyData.Health;
            Debug.Log("Initialize Enemy");
        }
        //If there is NO abilities, uses only this attack (Debugging)
        if (Abilities == null)
        {
            Abilities = new List<AbilityStruct>();
            Abilities.Add(new AbilityStruct{ name = "Struggle", damage = 5, probability = 1f, cooldown = 0, chargeTime = 40});
        }
    }
    
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


