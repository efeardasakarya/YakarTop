using System.Collections;
using UnityEngine;

public class DodgeballEnemy : MonoBehaviour
{
    public Transform player;
    public GameObject ballPrefab;
    public Transform handTransform;
    public float moveRange = 3f;
    public float moveSpeed = 2f;
    public float throwForce = 15f;
    public float minHoldTime = 1f;
    public float maxHoldTime = 3f;

    private Vector3 startPos;
    private bool movingRight = true;
    private bool canThrow = false;
    private bool canMove = true;

    void Start()
    {
        startPos = transform.position;
        DodgeballThrowManager.Instance.RegisterEnemy(this);
    }

    void Update()
    {
        if (canMove)
        {
            MoveSideToSide();
        }
    }

    void MoveSideToSide()
    {
        float moveDir = movingRight ? 1 : -1;
        transform.Translate(Vector3.right * moveDir * moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, startPos) > moveRange)
        {
            movingRight = !movingRight;
        }
    }

    public void BeginThrow()
    {
        canThrow = true;
        StartCoroutine(ThrowRoutine());
    }

    IEnumerator ThrowRoutine()
    {
        if (!canThrow) yield break;

        canMove = false; // Hareketi durdur
        GameObject ball = Instantiate(ballPrefab, handTransform.position, Quaternion.identity);
        ball.transform.SetParent(handTransform); // Top elde dursun

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        float holdTime = Random.Range(minHoldTime, maxHoldTime);
        yield return new WaitForSeconds(holdTime);

        if (ball == null) yield break;

        ball.transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            // Daha hassas nişan için pozisyonu yeniden hesapla
            Vector3 direction = (player.position - handTransform.position).normalized;
            rb.linearVelocity = direction * throwForce;
        }

        canThrow = false;

        yield return new WaitForSeconds(0.5f);
        canMove = true; // Hareket tekrar başlasın

        DodgeballThrowManager.Instance.EnemyFinishedThrowing();
    }
}
