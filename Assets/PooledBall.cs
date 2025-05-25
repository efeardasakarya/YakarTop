
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PooledBall : MonoBehaviour
{
    private Rigidbody rb;
    private float lifeTime = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        // Start return timer
        Invoke(nameof(ReturnToPool), lifeTime);
    }

    void OnDisable()
    {
        // Cancel invoke when disabled
        CancelInvoke();
    }

    private void ReturnToPool()
    {
        BallPool.Instance.Despawn(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Immediately return on any collision
        ReturnToPool();
    }
}



