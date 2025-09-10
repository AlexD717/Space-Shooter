using UnityEngine;

public class Asteroid : MonoBehaviour {

    private float health;

    private const float healthConst = 4;

    private void Start()
    {
        health = transform.parent.localScale.x * healthConst;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // TODO add destroy effect
    }
}
