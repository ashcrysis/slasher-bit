using UnityEngine;

using UnityEngine;

public class RandomPrefabInstantiator : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public GameObject[] prefabs; // Array to hold your 5 prefabs
    public string enemyTag = "Enemy"; // Tag for the enemies
    public float respawnCooldown = 2f; // Cooldown before respawning

    private int baseNumberOfPrefabs = 2; // Base number of prefabs per wave
    private int extraPrefabsPerWave = 2; // Number of extra prefabs added per wave
    private int currentWave = 1; // Current wave number
    private bool isRespawning = false; // Flag to track if respawning is in progress

 void Start()
{
    // Start by instantiating the base number of prefabs
    InstantiatePrefabsRandomly(baseNumberOfPrefabs);
}

    void Update()
    {
        // Check if all enemies are inactive and not currently respawning
        if (!isRespawning && AllEnemiesInactive())
        {
            // If all enemies are inactive and not respawning, start the respawn coroutine for the next wave
            StartCoroutine(RespawnAfterCooldown());
        }
    }

    void InstantiatePrefabsRandomly(int numberOfPrefabs)
    {
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            // Get a random index to choose a prefab from the array
            int randomPrefabIndex = Random.Range(0, prefabs.Length);

            // Get the range between start and end points
            float distance = Vector2.Distance(startPoint.position, endPoint.position);

            // Choose a random position along the range
            float randomPosition = Random.Range(0f, 1f);

            // Calculate the position based on the random value
            Vector2 spawnPosition = Vector2.Lerp(startPoint.position, endPoint.position, randomPosition);

            // Instantiate the randomly chosen prefab at the chosen position
            Instantiate(prefabs[randomPrefabIndex], spawnPosition, Quaternion.identity);
        }
    }

    bool AllEnemiesInactive()
    {
        // Check if all enemies are inactive
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        return enemies.Length <= 0;
    }

    System.Collections.IEnumerator RespawnAfterCooldown()
    {
        // Set the flag to indicate that respawning is in progress
        isRespawning = true;

        // Calculate the number of prefabs for the current wave
        int numberOfPrefabs = baseNumberOfPrefabs + (currentWave - 1) * extraPrefabsPerWave;

        // Wait for the respawn cooldown
        yield return new WaitForSeconds(respawnCooldown);

        // After waiting, increase the wave number and respawn the prefabs for the next wave
        currentWave++;
        Debug.Log("Wave " + currentWave + " - Spawning " + numberOfPrefabs + " enemies");

        InstantiatePrefabsRandomly(numberOfPrefabs);

        // Reset the flag after respawning is complete
        isRespawning = false;
    }
}
