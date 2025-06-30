using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{
    private static CombatManager _instance;
    private float _playerMaxHealth = 100000;
    private float _playerHealth;
    private float _playerDefense = 2;
    private float _playerDodge = 20;
    private float _playerBaseAttack = 1;
    [SerializeField] private Image abilityTimerBar;
    [SerializeField] private GameObject potionTimerGameObject;
    [SerializeField] private Image potionTimerBar;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject enemySpawn;
    [SerializeField] private MapPlayerClass mapPlayer;
    public EnemyDatabaseScriptable availableEnemies;
    private EnemyClass _currentEnemy;
    private AbilityStruct _currentAbility;
    
    private void Start()
    {
        StartCombat(SpawnRandomEnemy());
        _playerHealth = _playerMaxHealth;
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
        _currentEnemy = enemy;
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
        _currentAbility = _currentEnemy.DequeueAbility();
        float abilityTimer = _currentAbility.chargeTime;
        float timeLeft = abilityTimer + Time.time;
        while (Time.time <= timeLeft)
        {
            abilityTimerBar.fillAmount = (timeLeft - Time.time) / abilityTimer;
            yield return null;
        }
        DealDamageToPlayer(_currentAbility.damage);
        abilityTimerBar.fillAmount = 0f;

        _currentEnemy.AnimateEnemy(_currentAbility.animationName);
    }

    public void PotionUsed(PotionSo potion)
    {
        potionTimerGameObject.SetActive(true);
        PotionSo potionToUse = potion;
        StartCoroutine(PotionTimer(potionToUse));
    }
    
    private IEnumerator PotionTimer(PotionSo potion)
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
        if (_playerDodge <= Random.Range(0, 100))
        {
            healthBar.fillAmount = (_playerHealth - (damage - _playerDefense)) / _playerMaxHealth;
            _playerHealth -= damage;
            Debug.Log("Player received " + damage + " damage");
            if (_playerHealth <= 0)
            {
                if (mapPlayer != null)
                {
                    mapPlayer.PlayAnimation("Death");
                }

                if (GameManager.Instance != null)
                {
                    StartCoroutine(WaitToLoadScene("GameOver"));
                }
            }
            else if (mapPlayer != null)
            {
                mapPlayer.PlayAnimation("Hit");
            }
        }
        else
        {
            if (mapPlayer != null)
            {
                mapPlayer.PlayAnimation("Dodge");
            }
            Debug.Log("Player Dodged the Attack!");
        }
    }

    public void MeleeAttack()
    {
        DealDamageToEnemy((int)_playerBaseAttack);
    }
    
    private void ExecutePotion(PotionSo potion)
    {
        switch (potion.EffectType)
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

    private void PlayerEffect(PotionSo potion)
    {
        switch (potion.EffectType)
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
    
    private void EnemyEffect(PotionSo potion)
    {
        switch (potion.EffectType)
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
        healthBar.fillAmount = (_playerHealth+potency) / _playerMaxHealth;
        _playerHealth += potency;
        Debug.Log("Player healed!");
    }

    private void DealDamageToEnemy(int damage)
    {
        _currentEnemy.enemyData.health -= damage * 2;
        Debug.Log("Enemy received " + damage * 2 + " Damage");
        Debug.Log("Enemy has " + _currentEnemy.enemyData.health + " Health");
        if (_currentEnemy.enemyData.health <= 0)
        {
            _currentEnemy.AnimateEnemy("Death");

            if (GameManager.Instance != null)
            {
                StartCoroutine(WaitToLoadScene("VictoryScreen"));
            }
        }
    }
    
    private IEnumerator WaitToLoadScene(string sceneToLoad)
    {
        yield return new WaitForSeconds(1f);

        GameManager.Instance.LoadScene(sceneToLoad);
    }

    private void ShieldPlayer(int potency)
    {
        _playerDefense += potency / 2;
        Debug.Log("Player defence Increased to " + _playerDefense);
    }
    
    private void DodgePlayer(int potency)
    {
        _playerDodge += potency / 2;
        Debug.Log("Player dodge increased to " + _playerDodge);
    }
    
    private void DamageEnemyViaPotion(int potency)
    {
        DealDamageToEnemy(potency);
    }
    
    private void VenomEnemyViaPotion(int potency)
    {
        DealDamageToEnemy(potency);
    }
    
    private void EarthElementalDamage(int potency)
    {
        DealDamageToEnemy(potency);
    }
    private void WaterElementalDamage(int potency)
    {
        DealDamageToEnemy(potency);
    }
    private void FireElementalDamage(int potency)
    {
        DealDamageToEnemy(potency);
    }
}

