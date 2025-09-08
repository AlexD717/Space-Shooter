using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public float range;

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
        // TODO play effect
    }
}
