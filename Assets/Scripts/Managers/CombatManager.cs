using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    private static CombatManager Instance;
    [SerializeField] private Image abilityTimerBar;
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
        while (Time.time < timeLeft)
        {
            abilityTimerBar.fillAmount = (timeLeft - Time.time) / abilityTimer;
            yield return null;
        }
        DealDamageToPlayer(currentAbility.name,currentAbility.damage);
        abilityTimerBar.fillAmount = 0f; 
        //currentAbility = null;
    }

    private void DealDamageToPlayer(string abilityName,float damage)
    {
        //Debug.Log("Enemy used: " + abilityName);
        //Debug.Log("PLAYER DAMAGED BY: " + damage);
    }
}

