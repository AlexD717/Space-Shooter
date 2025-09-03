using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Vector2 mapSize;
    [SerializeField] private float generationSteps;
    [SerializeField] private float scale;
    [SerializeField] private float asteroidSpawnThreshold;
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private float minSpacing;

    private Vector2 noiseOffset;

    private Dictionary<GameObject, float> placedAsteroids = new Dictionary<GameObject, float>();

    private void Start()
    {
        // Generate perlin noise for level generation
        float startGenTime = Time.time;
        Debug.Log("Starting generation");

        noiseOffset = new Vector2(Random.Range(0f, 9999f), Random.Range(0f, 9999f));

        for (int i = 0; i < generationSteps; ++i)
        {

            GenerateAsteroid();
        }

        float endGenTime = Time.time;
        Debug.Log(startGenTime);
        Debug.Log(endGenTime);
        Debug.Log("Generation Finished; Took " + (endGenTime - startGenTime).ToString());
    }

    private void GenerateAsteroid()
    {
        float x = Random.Range(-mapSize.x / 2f, mapSize.x / 2f);
        float y = Random.Range(-mapSize.y / 2f, mapSize.y / 2f);
        float perlinNoise = FractalNoise(x, y);
        Vector2 spawnPosition = new Vector2(x, y);
        Debug.Log(x.ToString() + " " + y.ToString() + " " + perlinNoise.ToString());

        if (perlinNoise < asteroidSpawnThreshold)
            return;

        // Avoid Collision with Other Asteroid
        foreach (KeyValuePair<GameObject, float> entry in placedAsteroids)
        {
            GameObject asteroidObj = entry.Key;
            float asteroidRadius = entry.Value;
            if (Vector2.Distance(spawnPosition, asteroidObj.transform.position) < minSpacing + asteroidRadius * 2)
                return;
        }

        // Spawn Asteroid
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));

        placedAsteroids.Add(asteroid, .505f);
    }

    private float FractalNoise(float x, float y)
    {
        float noise = Mathf.PerlinNoise((x + noiseOffset.x) / scale, (y + noiseOffset.y) / scale);
        return noise;
    }
}
