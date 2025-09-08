using UnityEngine;

public class Cannon : Weapon
{
    [SerializeField] private GameObject bulletPrefab;

    protected override void Shoot()
    {
        Debug.Log("Firing Cannon");
    }
}
