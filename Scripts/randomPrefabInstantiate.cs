using UnityEngine;

public class RandomPrefabInstantiator : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public GameObject[] prefabs; // Array to hold your 5 prefabs
    public int numberOfPrefabs = 5; // Change this value as needed

    void Start()
    {
        InstantiatePrefabsRandomly();
    }

    void InstantiatePrefabsRandomly()
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
}
