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
        FacePlayer();

        if (canMove)
        {
            MoveSideToSide();
        }
    }

    private void FacePlayer()
    {
        Vector3 lookPos = player.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
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

        canMove = false;

        // Topu elinde oluştur
        GameObject ball = Instantiate(ballPrefab, handTransform.position, Quaternion.identity);
        ball.transform.SetParent(handTransform);

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        float holdTime = Random.Range(minHoldTime, maxHoldTime);
        yield return new WaitForSeconds(holdTime);

        if (ball == null) yield break;

        // Topu elden bırak
        ball.transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            // Oyuncunun üst kısmını hedef al (baş civarı)
            Vector3 targetPos = player.position + Vector3.up * 1.5f;
            Vector3 throwDir = (targetPos - handTransform.position).normalized;

            // Kuvvet uygula
            rb.AddForce(throwDir * throwForce, ForceMode.VelocityChange);
        }

        canThrow = false;
        yield return new WaitForSeconds(0.5f);
        canMove = true;

        DodgeballThrowManager.Instance.EnemyFinishedThrowing();
    }
}
