using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float range;
    [HideInInspector] public float damage;

    [SerializeField] protected GameObject destroyEffect;

    private Vector2 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (Vector2.Distance(startPos, transform.position) > range)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }
    }
}
