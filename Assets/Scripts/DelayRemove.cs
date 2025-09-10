using UnityEngine;

public class DelayRemove : MonoBehaviour
{
    [SerializeField] private float destroyDelay;

    private float destroyTime;

    void Start()
    {
        destroyTime = Time.time + destroyDelay;
    }

    void Update()
    {
        if (Time.time >= destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
