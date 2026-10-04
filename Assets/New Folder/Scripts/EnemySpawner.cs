using System.Collections;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 1.5f;

    [Header("Wave Settings")]
    public int currentWave = 1;
    public int enemiesBaseCount = 3;
    public int enemiesIncreasePerWave = 2;
    public int maxWaves = 3;
    public float timeBetweenWaves = 4f;

    [Header("UI")]
    public TMP_Text waveText;

    [Header("References")]
    public Transform player;
    public PlayerHealth playerHealth;

    private bool isSpawningWave = false;
    private bool waveFinishedSpawning = false;
    private bool spawningStopped = false;

    private void Start()
    {
        StartCoroutine(StartWaveRoutine());
    }

    private void Update()
    {
        if (spawningStopped)
        {
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            StopSpawning();
            return;
        }

        if (waveFinishedSpawning && CountEnemiesAlive() == 0 && !isSpawningWave)
        {
            if (currentWave >= maxWaves)
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.WinGame();
                }

                return;
            }

            StartCoroutine(StartNextWaveRoutine());
        }
    }

    private IEnumerator StartWaveRoutine()
    {
        isSpawningWave = true;
        waveFinishedSpawning = false;

        UpdateWaveUI();

        int enemiesToSpawn = enemiesBaseCount + (currentWave - 1) * enemiesIncreasePerWave;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            if (spawningStopped || IsGameOver())
            {
                yield break;
            }

            SpawnEnemy();

            yield return new WaitForSeconds(spawnInterval);
        }

        waveFinishedSpawning = true;
        isSpawningWave = false;
    }

    private IEnumerator StartNextWaveRoutine()
    {
        isSpawningWave = true;

        yield return new WaitForSeconds(timeBetweenWaves);

        if (spawningStopped || IsGameOver())
        {
            yield break;
        }

        currentWave++;
        StartCoroutine(StartWaveRoutine());
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("EnemySpawner missing enemy prefab or spawn points.");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        if (spawnPoint == null)
        {
            Debug.LogWarning("EnemySpawner has an empty spawn point slot.");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

        if (enemyAI != null)
        {
            enemyAI.player = player;
            enemyAI.playerHealth = playerHealth;
        }
    }

    public void StopSpawning()
    {
        spawningStopped = true;
        isSpawningWave = false;
        waveFinishedSpawning = false;
        StopAllCoroutines();
    }

    private int CountEnemiesAlive()
    {
        return FindObjectsByType<EnemyAI>(FindObjectsSortMode.None).Length;
    }

    private bool IsGameOver()
    {
        return GameManager.Instance != null && GameManager.Instance.IsGameOver;
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = "WAVE " + currentWave;
        }
    }

    public void ReportEnemyDeath()
    {
        // Kept only so old EnemyDeathReporter scripts do not create errors.
    }
}