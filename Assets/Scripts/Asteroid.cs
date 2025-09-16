using UnityEngine;

public class Asteroid : MonoBehaviour {

    private const float healthConst = 4;
    private const float particleMult = 25;
    private const float itemSpawnMult = 0.5f;

    private float parentScale;
    private float health;

    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private GameObject dropsItem;

    private void Start()
    {
        parentScale = transform.parent.localScale.x;
        health = parentScale * healthConst;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (destroyEffect != null)
        {
            GameObject effect = Instantiate(destroyEffect, transform.position, Quaternion.identity);
            ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
            ParticleSystem.Burst burst = particleSystem.emission.GetBurst(0);
            burst.count = new ParticleSystem.MinMaxCurve(Mathf.Round(particleMult * parentScale));
            particleSystem.emission.SetBurst(0, burst);
            ParticleSystem.ShapeModule shape = particleSystem.shape;
            shape.radius = parentScale;
        }

        if (dropsItem != null)
        {
            int itemsToSpawn = Mathf.RoundToInt(parentScale * itemSpawnMult);
            for (int i = 0; i < itemsToSpawn; i++)
            {
                Vector2 spawnPoint = (Random.insideUnitCircle * parentScale) + (Vector2)transform.position;
                GameObject item = Instantiate(dropsItem, spawnPoint, Quaternion.Euler(0, 0, Random.Range(0, 360)));

            }

        }
    }
}
