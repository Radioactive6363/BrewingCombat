using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{
    private static CombatManager Instance;
    private float playerMaxHealth = 100;
    private float playerHealth;
    private float playerDefense = 2;
    private float playerDodge = 20;
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
        GameObject spawnedEnemy = Instantiate(enemyPrefab, enemySpawn.transform.position, enemyPrefab.transform.rotation);
        
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

        currentEnemy.AnimateEnemy(currentAbility.animationName);
    }

    public void PotionUsed(PotionSO potion)
    {
        potionTimerGameObject.SetActive(true);
        PotionSO potionToUse = potion;
        StartCoroutine(PotionTimer(potionToUse));
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
        if (playerDodge <= Random.Range(0, 100))
        {
            healthBar.fillAmount = (playerHealth-(damage-playerDefense)) / playerMaxHealth;
            playerHealth -= damage;
            Debug.Log("Player received " + damage + " damage");
        }
        else
        {
            Debug.Log("Player Dodged the Attack!");
        }
    }
    
    private void ExecutePotion(PotionSO potion)
    {
        switch (potion.typeOfEffect)
        {
            case(PotionEffectType.DamagePotion):
                EnemyEffect(potion);
                break;
            case(PotionEffectType.DodgePotion):
                PlayerEffect(potion);
                break;
            case(PotionEffectType.ShieldPotion):
                PlayerEffect(potion);
                break;
            case(PotionEffectType.HealingPotion):
                PlayerEffect(potion);
                break;
            case(PotionEffectType.VenomPotion):
                EnemyEffect(potion);
                break;
            case(PotionEffectType.EarthElemental):
                EnemyEffect(potion);
                break;
            case(PotionEffectType.WaterElemental):
                EnemyEffect(potion);
                break;
            case(PotionEffectType.FireElemental):
                EnemyEffect(potion);
                break; 
        }
    }

    private void PlayerEffect(PotionSO potion)
    {
        switch (potion.typeOfEffect)
        {
            case(PotionEffectType.HealingPotion):
                HealDamageToPlayer(potion.Potency);
                break;
            case(PotionEffectType.ShieldPotion):
                ShieldPlayer(potion.Potency);
                break;
            case(PotionEffectType.DodgePotion):
                DodgePlayer(potion.Potency);
                break;
        }
    }
    
    private void EnemyEffect(PotionSO potion)
    {
        switch (potion.typeOfEffect)
        {
            case(PotionEffectType.DamagePotion):
                DamageEnemyViaPotion(potion.Potency);
                break;
            case(PotionEffectType.VenomPotion):
                VenomEnemyViaPotion(potion.Potency);
                break;
            case(PotionEffectType.EarthElemental):
                EarthElementalDamage(potion.Potency);
                break;
            case(PotionEffectType.WaterElemental):
                WaterElementalDamage(potion.Potency);
                break;
            case(PotionEffectType.FireElemental):
                FireElementalDamage(potion.Potency);
                break; 
        }
    }
    
    private void HealDamageToPlayer(int potency)
    {
        healthBar.fillAmount = (playerHealth+potency) / playerMaxHealth;
        playerHealth += potency;
        Debug.Log("Player healed!");
    }
    
    private void ShieldPlayer(int potency)
    {
        playerDefense += potency / 2;
        Debug.Log("Player defence Increased to " + playerDefense);
    }
    
    private void DodgePlayer(int potency)
    {
        playerDodge += potency / 2;
        Debug.Log("Player dodge increased to " + playerDodge);
    }
    
    private void DamageEnemyViaPotion(int potency)
    {
        currentEnemy.enemyData.Health -= potency;
        Debug.Log("Enemy received" + potency + " Damage");
    }
    
    private void VenomEnemyViaPotion(int potency)
    {
        currentEnemy.enemyData.Health -= potency / 2;
        Debug.Log("Enemy received" + potency + " Damage");
    }
    
    private void EarthElementalDamage(int potency)
    {
        currentEnemy.enemyData.Health -= potency / 2;
        Debug.Log("Enemy received" + potency + " Damage");
    }
    private void WaterElementalDamage(int potency)
    {
        currentEnemy.enemyData.Health -= potency / 2;
        Debug.Log("Enemy received" + potency + " Damage");
    }
    private void FireElementalDamage(int potency)
    {
        currentEnemy.enemyData.Health -= potency / 2;
        Debug.Log("Enemy received" + potency + " Damage");
    }
}

