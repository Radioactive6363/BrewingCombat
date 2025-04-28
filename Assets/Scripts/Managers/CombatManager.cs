using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    private static CombatManager Instance;
    private float playerMaxHealth = 100;
    private float playerHealth;
    [SerializeField] private Image abilityTimerBar;
    [SerializeField] private GameObject potionTimerGameObject;
    [SerializeField] private Image potionTimerBar;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject enemySpawn;
    public EnemyDatabaseScriptable availableEnemies;
    private EnemyClass currentEnemy;
    private AbilityStruct currentAbility;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Avoids destroying this object when reloading scene
        }
        else
        {
            Destroy(gameObject); // If instance already exists, destroy this one
        }
    }
    
    private void Start()
    {
        StartCombat(SpawnRandomEnemy());
        playerHealth = playerMaxHealth;
    }

    private EnemyClass SpawnRandomEnemy()
    {
        if (availableEnemies == null || availableEnemies.enemies.Length == 0)
        {
            Debug.LogError("No enemies available to spawn.");
        }
        
        // Select a random enemy prefab 
        int randomIndex = UnityEngine.Random.Range(0,availableEnemies.enemies.Length);
        GameObject enemyPrefab = availableEnemies.enemies[randomIndex];
        
        // Instantiate the enemy prefab in the scene
        GameObject spawnedEnemy = Instantiate(enemyPrefab, enemySpawn.transform.position, Quaternion.identity);
        
        // Get the EnemyClass component from the instantiated prefab
        EnemyClass enemy = spawnedEnemy.GetComponent<EnemyClass>();
        if (enemy == null)
        {
            Debug.LogError("Spawned enemy prefab does not contain an EnemyClass component.");
            return null; // Safely return null if something goes wrong
        }
        return enemy;
    }
    
    private void StartCombat(EnemyClass enemy)
    {
        currentEnemy = enemy;
        StartCoroutine(CombatLoop());
    }

    private IEnumerator CombatLoop()
    {
        while (true)
        {
            yield return StartCoroutine(ExecuteEnemyAbility());
        }
    }

    private IEnumerator ExecuteEnemyAbility()
    {
        //Basic Timing System using Time.time (real time unity)
        currentAbility = currentEnemy.DequeueAbility();
        float abilityTimer = currentAbility.chargeTime;
        float timeLeft = abilityTimer + Time.time;
        while (Time.time <= timeLeft)
        {
            abilityTimerBar.fillAmount = (timeLeft - Time.time) / abilityTimer;
            yield return null;
        }
        DealDamageToPlayer(currentAbility.damage);
        abilityTimerBar.fillAmount = 0f; 
    }

    public void PotionUsed(PotionSO potion)
    {
        potionTimerGameObject.SetActive(true);
        StartCoroutine(PotionTimer(potion));
    }
    
    private IEnumerator PotionTimer(PotionSO potion)
    {
        float potionCraftingTimer = potion.ChargeTime;
        float timeLeft = potionCraftingTimer + Time.time;
        while (Time.time < timeLeft)
        {
            potionTimerBar.fillAmount = (timeLeft - Time.time) / potionCraftingTimer;
            yield return null;
        }
        ExecutePotion(potion);
        potionTimerBar.fillAmount = 0f;
    }
    
    private void DealDamageToPlayer(int damage)
    {
        healthBar.fillAmount = (playerHealth-damage) / playerMaxHealth;
        playerHealth -= damage;
    }
    
    private void ExecutePotion(PotionSO potion)
    {
        switch (potion.typeOfEffect)
        {
            case(PotionEffectType.DamagePotion):
                break;
            case(PotionEffectType.DodgePotion):
                break;
            case(PotionEffectType.ShieldPotion):
                break;
            case(PotionEffectType.HealingPotion):
                break;
            case(PotionEffectType.VenomPotion):
                break;
            case(PotionEffectType.EarthElemental):
                break;
            case(PotionEffectType.WaterElemental):
                break;
            case(PotionEffectType.FireElemental):
                break; 
        }
    }
}

