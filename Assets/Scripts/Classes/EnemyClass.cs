using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyClass : MonoBehaviour, IAbilityQueue
{
    public EnemyData enemyData;
    [FormerlySerializedAs("Abilities")] [SerializeField] public List<AbilityStruct> abilities = new List<AbilityStruct>();

    private IAbilityQueue _abilityQueueHandler;  // Now using the queue handler
    private int _currentHealth;

    private Animator _animator;

    private void Start()
    {
        SetupEnemyData();
        SetupDefaultAbilities();

        _animator = GetComponent<Animator>();

        // Use the queue handler instead of the stack handler
        _abilityQueueHandler = new AbilityQueueHandler();  // Changed to the Queue handler
        InitializeQueue();  // Initialize the queue
        FillAbilityQueue();  // Fill the queue with abilities
    }

    public void Initialize(EnemyData data)
    {
        enemyData = data.Clone();
    }

    private void SetupEnemyData()
    {
        if (enemyData != null)
        {
            gameObject.name = enemyData.name;
            _currentHealth = enemyData.health;
            Debug.Log("Initialized Enemy: " + gameObject.name);
        }
    }

    private void SetupDefaultAbilities()
    {
        if (abilities.Count == 0)
        {
            abilities.Add(new AbilityStruct
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
            //Debug.Log("Queued Ability: " + chosenAbility.name);
        }
    }

    private AbilityStruct GetRandomAbility()
    {
        AbilityStruct chosenAbility = abilities[0];
        bool foundAbility = false;

        // Try up to 3 attempts
        for (int attempt = 0; attempt < 3; attempt++)
        {
            foreach (AbilityStruct ability in abilities)
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
        _abilityQueueHandler.QueueAbility(ability);
    }

    // This dequeues the ability (FIFO)
    public AbilityStruct DequeueAbility()
    {
        if (!CheckEmptyQueue())  // If the queue is not empty
        {
            var ability = _abilityQueueHandler.DequeueAbility();
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
        return _abilityQueueHandler.CheckEmptyQueue();
    }

    // Initialize the queue handler (in case you want to reset the queue)
    public void InitializeQueue()
    {
        _abilityQueueHandler.InitializeQueue();
    }

    public void AnimateEnemy(string animationName)
    {
        if (_animator != null)
        {
            _animator.Play(animationName);
        }
    }
}
