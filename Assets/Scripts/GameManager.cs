using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //singleton
    public static GameManager _GameManager;
    
    [Header("Player System")]
    public Player player;

    [Header("Enemy System")]
    public GameState gameState = GameState.roundStart;
    public GameObject enemyPrefab;

    [Header("Drop System")]
    [Range(0f, 1f)]
    public float dropDecayRate = 0.1f;
    public List<Drop> possibleDrops;

    ObjectPool<Enemy> enemyPool;

    public BoxCollider[] spawnAreas;
    public int candyCornCount;

    [Header("UI")]
    public TMP_Text candyCornText;
    public Image healthBar;
    public Image overShieldBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //Singleton
        if (_GameManager == null)
        {
            _GameManager = this;
        }

        enemyPool = new ObjectPool<Enemy>(() =>
        {
            enemyPrefab = Instantiate(enemyPrefab);
            Enemy enemy = enemyPrefab.GetComponent<Enemy>();
            enemy.Health.OnDeath.AddListener(() => enemyPool.Release(enemy));
            enemyPrefab.SetActive(false);
            return enemyPrefab.GetComponent<Enemy>();
        }, defaultCapacity: 50);

        StartRound(10);

        SetupDropQueue();

        player.Health.OnHealthChanged.AddListener(HealthListener);
    }



    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.round)
        {
            //check if all enemies are dead
            if (enemyPool.CountActive == 0)
            {
                gameState = GameState.roundEnd;
            }
        }
        else if (gameState == GameState.roundEnd)
        {
            Debug.Log("Round End");
            gameState = GameState.roundStart;
            StartCoroutine(RoundEnd());
        }
    }

    IEnumerator RoundEnd()
    {
        yield return new WaitForSeconds(5.0f);
        StartRound();
        gameState = GameState.round;
    }



    public void StartRound(float numberOfEnemies = 10)
    {
        //spawn enemies
        foreach (BoxCollider spawnArea in spawnAreas)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x), 0, UnityEngine.Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z));
                Enemy enemy = enemyPool.Get();
                enemy.Health.ResetHealth();
                enemy.transform.position = spawnPosition;
                enemy.gameObject.SetActive(true);
            }
        }
        gameState = GameState.round;
    }
    private void SetupDropQueue()
    {
        for (int i = 0; i < possibleDrops.Count; i++)
        {
            var drop = possibleDrops[i];
            drop.currentDropChance = drop.initialDropChance;
            possibleDrops[i] = drop;
        }
    }
    public Drop GetWeightedRandomDrop()
    {
        // Calculate total drop chance
        // 50 + 50 = 100
        float totalChance = 0f;
        foreach (var drop in possibleDrops)
        {
            totalChance += drop.currentDropChance;
        }

        if (totalChance <= 0f)
        {
            // All drop chances are zero, cannot select a drop
            //reset drop chances
            SetupDropQueue();
            foreach (var drop in possibleDrops)
            {
                totalChance += drop.currentDropChance;
            }
        }

        // Generate a random number between 0 and totalChance
        //supose totalChance is 100 and randomValue is 50
        float randomValue = UnityEngine.Random.value * totalChance;
        float cumulativeChance = 0f;

        // Select a drop based on weighted chance
        foreach (var drop in possibleDrops)
        {
            //The first drop adds a 50% chance to the cumulativeChance
            //That value is <= 50 so it is selected
            //however the drop chance is decreased by 10% so the next time it is selected it will have a 40% chance
            //The next time it is selected it will have a 40% chance
            cumulativeChance += drop.currentDropChance;
            if (randomValue <= cumulativeChance)
            {
                // Decrease the current drop chance of the selected drop
                DecreaseDropChance(drop);
                return drop;
            }
        }

        // Fallback in case of rounding errors
        // Standard random selection
        Debug.LogWarning("Fell through weighted drop selection, falling back to random selection");
        int randomIndex = UnityEngine.Random.Range(0, possibleDrops.Count);
        return possibleDrops[randomIndex];
    }

    private void DecreaseDropChance(Drop drop)
    {
        // Decrease by 10% of the initial drop chance
        float decreaseAmount = drop.initialDropChance * 0.1f;
        drop.currentDropChance -= decreaseAmount;

        //Debug.Log("Decreased drop chance of " + drop.dropPrefab.name + " to " + drop.currentDropChance);

        // Ensure the drop chance doesn't go below zero
        if (drop.currentDropChance < 0f)
        {
            drop.currentDropChance = 0f;
        }
    }

    internal void AddCandyCorn()
    {
        this.candyCornCount++;
        candyCornText.text = candyCornCount.ToString();
    }

    private void HealthListener(Health health){
        healthBar.fillAmount = health.NormalizedHealth;
    }
}

public enum GameState
{
    roundStart,
    round,
    roundEnd,
    gameOver

}

[Serializable]
public class Drop
{
    public GameObject dropPrefab;
    public float initialDropChance;

    public float currentDropChance;

}