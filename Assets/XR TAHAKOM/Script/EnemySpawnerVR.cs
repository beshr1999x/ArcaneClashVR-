using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class EnemySpawnerVR : MonoBehaviour
{
    public List<GameObject> enemiesPrefabs;
    public List<EnemyManger> enemies;

    public Wave[] waves;
    private Wave currentWave;
    private int currentWaveIndex;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI scoreText;

    private float timeBetweenSpawns;
    private bool stopSpawning = false;
    private float playerScore = 0f;
    private int previousSpawnPoint = -1;

    private void Awake()
    {
        currentWave = waves[currentWaveIndex];
        timeBetweenSpawns = currentWave.TimeBeforeThisWave;
    }

    private void Start()
    {
        enemies.Clear();

        // Create object pools for enemies
        foreach (GameObject enemyPrefab in enemiesPrefabs)
        {
            string enemyType = enemyPrefab.GetComponent<EnemyControllerVR>().enemyType.ToString();
            ObjectpoolVR.Instance.CreatePool(enemyType, enemyPrefab, 10);
        }

        // Update text UI
        if (waveText != null)
        {
            waveText.text = $"Wave {currentWaveIndex + 1}";
        }

        if (scoreText != null)
        {
            scoreText.text = $"Score: {playerScore}";
        }

        // Start spawning waves
        StartCoroutine(SpawnWaveWithDelay());
    }

    private void Update()
    {
        // Check for defeated enemies and increment player's score
        List<EnemyManger> defeatedEnemies = enemies.FindAll(enemy => enemy.enemyGameObject == null || !enemy.enemyGameObject.activeSelf);
        foreach (EnemyManger defeatedEnemy in defeatedEnemies)
        {
            GameObject enemyPrefab = currentWave.EnemiesInWave.FirstOrDefault(prefab => prefab != null && prefab.GetComponent<EnemyControllerVR>() != null && prefab.GetComponent<EnemyControllerVR>().enemyType == defeatedEnemy.enemyController.enemyType);

            if (enemyPrefab != null && defeatedEnemy.enemyController != null)
            {
                int scoreValue = defeatedEnemy.enemyController.scoreValue;
                playerScore += scoreValue;
                if (scoreText != null)
                {
                    scoreText.text = $"Score: {playerScore}";
                }
                enemies.Remove(defeatedEnemy);
            }
        }
    }

    public IEnumerator SpawnWaveWithDelay()
    {
        while (!stopSpawning)
        {
            yield return StartCoroutine(SpawnWave());
            yield return new WaitForSeconds(2f); // Wait for 2 seconds before starting the next wave

            // Check if most enemies have been destroyed
            if (enemies.Count <= 1)
            {
                IncrementWave();
            }
            else
            {
                // Wait until most enemies are destroyed
                yield return new WaitUntil(() => enemies.Count <= 1);
                IncrementWave();
            }
        }
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentWave.NumberToSpawn; i++)
        {
            GameObject enemyPrefab = currentWave.EnemiesInWave[Random.Range(0, currentWave.EnemiesInWave.Length)];
            int randSpawnPoint = Random.Range(0, transform.childCount);

            // Make sure no two consecutive enemies are spawned in the same track
            do
            {
                randSpawnPoint = Random.Range(0, transform.childCount);
            } while (randSpawnPoint == previousSpawnPoint && transform.childCount > 1);

            string enemyType = enemyPrefab.GetComponent<EnemyControllerVR>().enemyType.ToString();

            // Get the enemy from the object pool
            GameObject spawnedEnemy = ObjectpoolVR.Instance.GetObjectFromPool(enemyType);

            if (spawnedEnemy != null)
            {
                spawnedEnemy.transform.SetParent(transform.GetChild(randSpawnPoint));
                spawnedEnemy.transform.localPosition = Vector3.zero;
                EnemyControllerVR enemyController = spawnedEnemy.GetComponent<EnemyControllerVR>();
                enemies.Add(new EnemyManger
                {
                    enemyGameObject = spawnedEnemy,
                    enemyController = enemyController
                });
            }

            previousSpawnPoint = randSpawnPoint;

            // Wait a bit between enemies
            float randWait = Random.Range(0.5f, 1.5f);
            yield return new WaitForSeconds(randWait);
        }

        // Reset the previousSpawnPoint after the wave is spawned
        previousSpawnPoint = -1;
    }

    private void IncrementWave()
    {
        if (currentWaveIndex + 1 < waves.Length)
        {
            currentWaveIndex++;
            currentWave = waves[currentWaveIndex];

            // Clear the enemies list before spawning a new wave
            enemies.Clear();

            // Update the wave text UI
            if (waveText != null)
            {
                waveText.text = $"Wave:  {currentWaveIndex + 1}";
            }

            // Update wave difficulty based on player progress
            if (playerScore >= currentWave.ScoreThresholdForNextWave)
            {
                float newNumberToSpawn = currentWave.NumberToSpawn + (currentWave.NumberToSpawn * 0.2f);
                currentWave.SetNumberToSpawn(newNumberToSpawn);

                float newTimeBeforeWave = currentWave.TimeBeforeThisWave - 0.2f;
                currentWave.SetTimeBeforeThisWave(newTimeBeforeWave);
            }
            else
            {
                float newNumberToSpawn = currentWave.NumberToSpawn - (currentWave.NumberToSpawn * 0.1f);
                currentWave.SetNumberToSpawn(newNumberToSpawn);

                float newTimeBeforeWave = currentWave.TimeBeforeThisWave + 0.1f;
                currentWave.SetTimeBeforeThisWave(newTimeBeforeWave);
            }
        }
        else
        {
            stopSpawning = true;
        }
    }

    public void HandleEnemyDestruction(EnemyControllerVR enemyController)
    {
        // Find the corresponding Enemy instance in the enemies list
        EnemyManger enemy = enemies.Find(e => e.enemyController == enemyController);

        if (enemy != null)
        {
            // Update the player's score based on the enemy's scoreValue
            playerScore += enemy.enemyController.scoreValue;
            if (scoreText != null)
            {
                scoreText.text = $"Score: {playerScore}";
            }

            // Remove the enemy from the enemies list
            enemies.Remove(enemy);
        }
    }
}
