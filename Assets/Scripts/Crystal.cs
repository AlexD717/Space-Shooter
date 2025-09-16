using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private float magnetDistance;
    [SerializeField] private float moveSpeed;

    private Transform player;
    private bool detectedByPlayer = false;

    void Start()
    {
        player = FindFirstObjectByType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;

        if (detectedByPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.position) < 0.2)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, player.position) <= magnetDistance)
            {
                detectedByPlayer = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, magnetDistance);
    }
}
