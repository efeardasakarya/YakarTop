using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float changeDirectionTime = 2f;
    public Vector2 movementRange = new Vector2(5f, 5f);
    public Vector2 areaSize = new Vector2(10f, 10f); // NPC'nin hareket edebileceði alanýn boyutu
    private Vector3 targetDirection;
    private float timer;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        SetRandomDirection();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 || IsOutOfBounds())
        {
            SetRandomDirection();
        }

        Move();
    }

    void SetRandomDirection()
    {
        float randomX = Random.Range(-movementRange.x, movementRange.x);
        float randomZ = Random.Range(-movementRange.y, movementRange.y);
        targetDirection = new Vector3(randomX, 0, randomZ).normalized;
        timer = changeDirectionTime;
    }

    void Move()
    {
        Vector3 newPosition = transform.position + targetDirection * moveSpeed * Time.deltaTime;
        if (IsWithinBounds(newPosition))
        {
            transform.position = newPosition;
        }
        else
        {
            SetRandomDirection();
        }
    }

    bool IsWithinBounds(Vector3 position)
    {
        return position.x >= startPosition.x - areaSize.x / 2 &&
               position.x <= startPosition.x + areaSize.x / 2 &&
               position.z >= startPosition.z - areaSize.y / 2 &&
               position.z <= startPosition.z + areaSize.y / 2;
    }

    bool IsOutOfBounds()
    {
        return !IsWithinBounds(transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            
            Destroy(gameObject);
        }
    }
}
