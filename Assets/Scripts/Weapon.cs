using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float cooldown;
    [SerializeField] private float range;
    [SerializeField] private float speed;

    private float cooldownTimer;

    public virtual void Fire()
    {
        if (Time.time < cooldownTimer) return;

        Shoot();
        cooldownTimer = Time.time + cooldown;
    }

    protected abstract void Shoot();
}
