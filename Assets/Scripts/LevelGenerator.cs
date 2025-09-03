using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Map Generation")]
    [SerializeField] private Vector2 mapSize;
    [SerializeField] private float generationSteps;
    [SerializeField] private float scale;
    [SerializeField] private float asteroidSpawnThreshold;
    [SerializeField] private float minSpacing;

    [Header("Asteroid Config")]
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private AnimationCurve asteroidSizeDestribution;
    [SerializeField] private Vector2 asteroidSizeRange;

    [Header("Initial Settings")]
    [SerializeField] private Transform asteroidParent;

    private Vector2 noiseOffset;
    private Dictionary<GameObject, float> placedAsteroids = new Dictionary<GameObject, float>();

    private void Start()
    {
        // Generate perlin noise for level generation
        float startGenTime = Time.realtimeSinceStartup;

        noiseOffset = new Vector2(Random.Range(0f, 9999f), Random.Range(0f, 9999f));

        for (int i = 0; i < generationSteps; ++i)
        {

            GenerateAsteroid();
        }

        float endGenTime = Time.realtimeSinceStartup;
        Debug.Log("Map Generation Finished; Took " + (endGenTime - startGenTime).ToString());
    }

    private void GenerateAsteroid()
    {
        float x = Random.Range(-mapSize.x / 2f, mapSize.x / 2f);
        float y = Random.Range(-mapSize.y / 2f, mapSize.y / 2f);
        float perlinNoise = PerlinNoise(x, y);
        Vector2 spawnPosition = new Vector2(x, y);
        
        if (perlinNoise < asteroidSpawnThreshold)
            return;

        float asteroidRadius = Mathf.Lerp(asteroidSizeRange.x, asteroidSizeRange.y, asteroidSizeDestribution.Evaluate(Random.value));

        // Avoid Collision with Other Asteroid
        foreach (KeyValuePair<GameObject, float> entry in placedAsteroids)
        {
            GameObject spawnedAsteroidObj = entry.Key;
            float spawnedAsteroidRadius = entry.Value;
            if (Vector2.Distance(spawnPosition, spawnedAsteroidObj.transform.position) < minSpacing + spawnedAsteroidRadius + asteroidRadius)
                return;
        }

        // Spawn Asteroid
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
        asteroid.transform.localScale = Vector2.one * asteroidRadius;
        asteroid.transform.parent = asteroidParent;

        placedAsteroids.Add(asteroid, asteroidRadius);
    }

    private float PerlinNoise(float x, float y)
    {
        float noise = Mathf.PerlinNoise((x + noiseOffset.x) / scale, (y + noiseOffset.y) / scale);
        return noise;
    }
}
