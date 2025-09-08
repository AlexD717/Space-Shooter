using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float range;
    [SerializeField] protected float speed;

    private float cooldownTimer;

    public virtual void Fire()
    {
        if (Time.time < cooldownTimer) return;

        Shoot();
        cooldownTimer = Time.time + cooldown;
    }

    protected abstract void Shoot();

    public virtual Transform[] GetChildrenArray()
    {
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }
        return children;
    }

}
