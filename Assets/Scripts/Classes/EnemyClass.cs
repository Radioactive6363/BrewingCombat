using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyClass : MonoBehaviour, IAbilityQueue
{
    public EnemyData enemyData;
    [SerializeField] private List<AbilityStruct> Abilities = new List<AbilityStruct>();

    private IAbilityQueue abilityQueueHandler;  // Now using the queue handler
    private int _currentHealth;

    private void Start()
    {
        SetupEnemyData();
        SetupDefaultAbilities();

        // Use the queue handler instead of the stack handler
        abilityQueueHandler = new AbilityQueueHandler();  // Changed to the Queue handler
        InitializeQueue();  // Initialize the queue
        FillAbilityQueue();  // Fill the queue with abilities
    }

    public void Initialize(EnemyData data)
    {
        enemyData = data;
    }

    private void SetupEnemyData()
    {
        if (enemyData != null)
        {
            gameObject.name = enemyData.Name;
            _currentHealth = enemyData.Health;
            Debug.Log("Initialized Enemy: " + gameObject.name);
        }
    }

    private void SetupDefaultAbilities()
    {
        if (Abilities.Count == 0)
        {
            Abilities.Add(new AbilityStruct
            {
                name = "Struggle",
                damage = 5,
                probability = 5,
                cooldown = 0,
                chargeTime = 40
            });
        }
    }

    private void FillAbilityQueue()
    {
        for (int i = 0; i < 10; i++)  // Fill the queue with 10 abilities
        {
            AbilityStruct chosenAbility = GetRandomAbility();
            // Enqueue the chosen ability
            QueueAbility(chosenAbility);
            Debug.Log("Queued Ability: " + chosenAbility.name);
        }
    }

    private AbilityStruct GetRandomAbility()
    {
        AbilityStruct chosenAbility = Abilities[0];
        bool foundAbility = false;

        // Try up to 3 attempts
        for (int attempt = 0; attempt < 3; attempt++)
        {
            foreach (AbilityStruct ability in Abilities)
            {
                if (Random.Range(0f, ability.probability) <= 1f)
                {
                    chosenAbility = ability;
                    foundAbility = true;
                    break;
                }
            }

            if (foundAbility)
                break;
        }

        return chosenAbility;
    }

    // This will enqueue the ability (FIFO)
    public void QueueAbility(AbilityStruct ability)
    {
        abilityQueueHandler.QueueAbility(ability);
    }

    // This dequeues the ability (FIFO)
    public AbilityStruct DequeueAbility()
    {
        if (!CheckEmptyQueue())  // If the queue is not empty
        {
            var ability = abilityQueueHandler.DequeueAbility();
            Debug.Log($"Dequeued Ability: {ability.name}");

            // Refill the empty spot of the queue
            AbilityStruct chosenAbility = GetRandomAbility();
            QueueAbility(chosenAbility);
            Debug.Log("Queued Ability: " + chosenAbility.name);

            return ability;
        }
        else
        {
            Debug.LogWarning("Attempted to unstack from an empty queue!");
            return default;
        }
    }

    // Check if the queue is empty
    public bool CheckEmptyQueue()
    {
        return abilityQueueHandler.CheckEmptyQueue();
    }

    // Initialize the queue handler (in case you want to reset the queue)
    public void InitializeQueue()
    {
        abilityQueueHandler.InitializeQueue();
    }
}
