using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyClass : MonoBehaviour
{
    public EnemyData enemyData;
    [SerializeField] private List<AbilityStruct> Abilities;
    private int _currentHealth;

    public void Initialize(EnemyData data)
    {
        enemyData = data;
    }
    
    private void Start()
    {
        SetupEnemyData();
        SetupDefaultAbilities();
    }

    //References Enemy Data
    private void SetupEnemyData()
    {
        if (enemyData != null)
        {
            gameObject.name = enemyData.Name;
            _currentHealth = enemyData.Health;
            Debug.Log("Initialize Enemy");
        }
    }
    
    //If there is NO abilities, uses only this attack (Debugging)
    private void SetupDefaultAbilities()
    {
        if (Abilities == null)
        {
            Abilities = new List<AbilityStruct>();
        }
        
        if (Abilities.Count == 0)
        {
            Abilities.Add(new AbilityStruct
            {
                name = "Struggle",
                damage = 5,
                probability = 1f,
                cooldown = 0,
                chargeTime = 40
            });
        }
    }
    
    //Random Ability Selector
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


