using UnityEngine;

public class CannonProjectile : Projectile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Asteroid asteroid = collision.GetComponent<Asteroid>();
        if (asteroid != null) {
            asteroid.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
