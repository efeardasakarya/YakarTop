using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float changeDirectionTime = 2f;
    public Vector2 areaSize = new Vector2(5f, 5f); // Manuel olarak belirleyeceðin alan
    public float catchChance = 0.2f; // %90 ihtimalle topu tutacak
    public int extraLives = 0; // NPC’nin ekstra can sayýsý
    public GameObject catchIndicator; // Kafasýnýn üstünde belirecek görsel

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float timer;
    private Animator animator;

    void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        SetNewTargetPosition();

        if (catchIndicator != null)
            catchIndicator.SetActive(false); // Baþlangýçta kapalý
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
        Debug.Log("Trigger'a giren obje: " + other.gameObject.name);

        if (other.CompareTag("Ball")) // Top NPC'ye çarptýðýnda
        {
            Debug.Log("Top geldi!");
            float randomValue = Random.value;
            Debug.Log("Random.value: " + randomValue);

            if (randomValue < catchChance) // % catchChance ihtimalle yakalama
            {
                CatchBall(other.gameObject);
            }
            else
            {
                GetHit();
            }
        }
    }

    void CatchBall(GameObject ball)
    {
        Debug.Log("NPC topu yakaladý!");
        extraLives++; // Ekstra can kazan
        Destroy(ball); // Topu sahneden kaldýr

        if (catchIndicator != null)
        {
            catchIndicator.SetActive(true); // Sprite aç
            // Invoke("HideCatchIndicator", 1.5f); // 1.5 saniye sonra kaybolsun
        }
    }

    void HideCatchIndicator()
    {
        if (catchIndicator != null)
            catchIndicator.SetActive(false);
    }

    void GetHit()
    {
        Debug.Log("NPC topa yakalandý!");
        // NPC vurulduðunda catchIndicator'ý gizle
        if (extraLives == 1)
        {
            if (catchIndicator != null)
                catchIndicator.SetActive(false);
            extraLives--;
            return;
        }
        if (extraLives < 1)
        {
            transform.position = new Vector3(0,0,0);
        }
        else
        {
            extraLives--;
        }
           
        // Burada NPC’nin vurulma durumunu iþleyebilirsin.
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(startPosition == Vector3.zero ? transform.position : startPosition, new Vector3(areaSize.x, 0.1f, areaSize.y));
    }
}
