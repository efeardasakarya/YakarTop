using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float changeDirectionTime = 2f;
    public Vector2 areaSize = new Vector2(5f, 5f); // Manuel olarak belirleyeceðin alan



    private bool isDead;


    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float timer;
    private Animator animator;

    void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        SetNewTargetPosition();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        MoveTowardsTarget();

        if (timer <= 0 || ReachedTargetPosition())
        {
            SetNewTargetPosition();
        }

        Vector3 direction = (targetPosition - transform.position).normalized;
        animator.SetFloat("Move X", direction.x);
        animator.SetFloat("Move Z", direction.z);
    }

    void SetNewTargetPosition()
    {
        float randomX = Random.Range(startPosition.x - areaSize.x / 2, startPosition.x + areaSize.x / 2);
        float randomZ = Random.Range(startPosition.z - areaSize.y / 2, startPosition.z + areaSize.y / 2);
        targetPosition = new Vector3(randomX, transform.position.y, randomZ);
        timer = changeDirectionTime;
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    bool ReachedTargetPosition()
    {
        return Vector3.Distance(transform.position, targetPosition) < 0.2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            transform.position = new Vector3(0, 0, 0);
            
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(startPosition == Vector3.zero ? transform.position : startPosition, new Vector3(areaSize.x, 0.1f, areaSize.y));
    }


}