using UnityEngine;

public class Cannon : Weapon
{
    [SerializeField] private GameObject bulletPrefab;

    private Transform[] firePoints;
    private Rigidbody2D playerRb;

    private void Start()
    {
        firePoints = GetChildrenArray();
        playerRb = transform.parent.gameObject.GetComponent<Rigidbody2D>();
    }

    protected override void Shoot()
    {
        foreach (Transform t in firePoints)
        {
            GameObject bullet = Instantiate(bulletPrefab, t.position, t.rotation);
            CannonProjectile projectile = bullet.GetComponent<CannonProjectile>();
            projectile.range = range;
            projectile.damage = damage;

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            Vector2 fireDir = t.up;
            bulletRb.linearVelocity = (fireDir * speed) + playerRb.linearVelocity;
        }
    }
}
