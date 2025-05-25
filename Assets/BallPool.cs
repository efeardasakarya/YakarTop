using System.Collections.Generic;
using UnityEngine;

public class BallPool : MonoBehaviour
{
    public static BallPool Instance;

    [Header("Pool Settings")]
    public GameObject ballPrefab;
    public int initialPoolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Pre-instantiate balls
        for (int i = 0; i < initialPoolSize; i++)
        {
            var obj = Instantiate(ballPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    /// <summary>
    /// Retrieves an inactive ball from the pool and activates it.
    /// </summary>
    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
            obj.SetActive(true);
        }
        else
        {
            // Expand pool if needed
            obj = Instantiate(ballPrefab);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;

        // Ensure physics state reset
        var rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        return obj;
    }

    /// <summary>
    /// Returns a ball to the pool (deactivates it).
    /// </summary>
    public void Despawn(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
