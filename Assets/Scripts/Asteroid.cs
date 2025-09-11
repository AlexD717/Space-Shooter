using UnityEngine;

public class Asteroid : MonoBehaviour {

    private float health;
    private const float healthConst = 4;
    private const float particleMult = 100;

    [SerializeField] private GameObject destroyEffect;

    private void Start()
    {
        health = transform.parent.localScale.x * healthConst;
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
            burst.count = new ParticleSystem.MinMaxCurve(Mathf.Round(particleMult * transform.localScale.x));
            particleSystem.emission.SetBurst(0, burst);
            ParticleSystem.ShapeModule shape = particleSystem.shape;
            shape.radius *= transform.localScale.x;
        }
    }
}
