using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Map Generation")]
    [SerializeField] private float blockSize;
    private HashSet<Vector2Int> generatedBlocks = new HashSet<Vector2Int>();
    private Dictionary<Vector2Int, int> generatingBlocks = new Dictionary<Vector2Int, int>();
    [SerializeField] private Vector2Int initialBlockRange;
    [SerializeField] private int viewDistance;
    [SerializeField] private int generationSteps;
    [SerializeField] private int framesToGenerateOver;
    [SerializeField] private float scale;
    [SerializeField] private float asteroidSpawnThreshold;
    [SerializeField] private float minSpacing;
    [SerializeField] private float noSpawnStarterArea;

    [Header("Asteroid Config")]
    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private AnimationCurve asteroidSizeDestribution;
    [SerializeField] private Vector2 asteroidSizeRange;

    [Header("Initial Settings")]
    [SerializeField] private Transform asteroidParent;

    private Vector2 noiseOffset;
    private Dictionary<GameObject, float> placedObjects = new Dictionary<GameObject, float>();
    private GameObject player;

    private void Start()
    {
        // Generate perlin noise for level generation
        float startGenTime = Time.realtimeSinceStartup;
        noiseOffset = new Vector2(Random.Range(0f, 9999f), Random.Range(0f, 9999f));

        // Avoid placing asteroids right on the player
        player = FindFirstObjectByType<Player>().gameObject;
        placedObjects.Add(player, noSpawnStarterArea / 2f);

        // Spawn all asteroids
        for (int x = -initialBlockRange.x; x <= initialBlockRange.x; x++)
        {
            for (int y = -initialBlockRange.y; y <= initialBlockRange.y; y++)
            {
                Vector2Int block = new Vector2Int(x, y);
                GenerateBlock(block, generationSteps);
                generatedBlocks.Add(block);
            }
        }

        float endGenTime = Time.realtimeSinceStartup;
        Debug.Log("Map Generation Finished; Took " + (endGenTime - startGenTime).ToString());
    }

    private void Update()
    {
        if (player == null)
            return;

        Vector2Int playerBlock = new Vector2Int(
            Mathf.FloorToInt(player.transform.position.x / blockSize),
            Mathf.FloorToInt(player.transform.position.y / blockSize)
        );

        // Identify if player is near the edge
        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int y = -viewDistance; y <= viewDistance; y++)
            {
                Vector2Int blockToGenerate = playerBlock + new Vector2Int(x, y);
                if (!generatedBlocks.Contains(blockToGenerate))
                {
                    GeneratePartialBlock(blockToGenerate);
                }
            }
        }
    }

    private void GeneratePartialBlock(Vector2Int blockToGenerate)
    {
        int stepsToGenerateFor = Mathf.RoundToInt(generationSteps / framesToGenerateOver);
        GenerateBlock(blockToGenerate, stepsToGenerateFor);

        if (generatingBlocks.ContainsKey(blockToGenerate))
        {
            int totalStepsGeneratedFor = generatingBlocks[blockToGenerate] + stepsToGenerateFor;
            if (totalStepsGeneratedFor >= generationSteps)
            {
                generatedBlocks.Add(blockToGenerate);
                generatingBlocks.Remove(blockToGenerate);
            }
            else
            {
                generatingBlocks[blockToGenerate] = totalStepsGeneratedFor;
            }
        }
        else
        {
            generatingBlocks.Add(blockToGenerate, stepsToGenerateFor);
        }
    }

    private void GenerateBlock(Vector2Int block, int generationSteps)
    {
        Vector2 bottomLeft = new Vector2(block.x * blockSize, block.y * blockSize);
        Vector2 topRight = bottomLeft + Vector2.one * blockSize;

        // Spawn all asteroids
        for (int i = 0; i < generationSteps; i++)
        {
            float x = Random.Range(bottomLeft.x, topRight.x);
            float y = Random.Range(bottomLeft.y, topRight.y);

            GenerateAsteroid(new Vector2(x, y));
        }
    }

    private void GenerateAsteroid(Vector2 spawnPosition)
    {
        float perlinNoise = PerlinNoise(spawnPosition);
        
        if (perlinNoise < asteroidSpawnThreshold)
            return;

        float asteroidRadius = Mathf.Lerp(asteroidSizeRange.x, asteroidSizeRange.y, asteroidSizeDestribution.Evaluate(Random.value));

        // Avoid Collision with Other Asteroid
        foreach (KeyValuePair<GameObject, float> entry in placedObjects)
        {
            GameObject spawnedAsteroidObj = entry.Key;
            float spawnedAsteroidRadius = entry.Value;
            if (Vector2.Distance(spawnPosition, spawnedAsteroidObj.transform.position) < minSpacing + spawnedAsteroidRadius + asteroidRadius)
                return;
        }

        // Spawn Asteroid
        GameObject asteroid = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)], spawnPosition, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
        asteroid.transform.localScale = Vector2.one * asteroidRadius;
        asteroid.transform.parent = asteroidParent;

        placedObjects.Add(asteroid, asteroidRadius);
    }

    private float PerlinNoise(Vector2 cords)
    {
        return Mathf.PerlinNoise((cords.x + noiseOffset.x) / scale, (cords.y + noiseOffset.y) / scale);
    }
}
