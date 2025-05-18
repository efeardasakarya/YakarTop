using System.Collections;
using UnityEngine;

public class DodgeballEnemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;               // Inspector’dan atanmalı veya tag ile bulunacak
    public GameObject ballPrefab;          // Rigidbody’li prefab
    public Transform handTransform;        // El pozisyonu

    [Header("Movement")]
    public float moveRange = 3f;
    public float moveSpeed = 2f;

    [Header("Throwing")]
    public float throwForce = 15f;
    public float minHoldTime = 1f;
    public float maxHoldTime = 3f;

    private Vector3 startPos;
    private bool movingRight = true;
    private bool canMove = true;

    void Awake()
    {
        startPos = transform.position;

        // 1) Player referansını otomatik al:
        if (player == null)
        {
            var pObj = GameObject.FindGameObjectWithTag("Runner");
            if (pObj != null)
            {
                player = pObj.transform;
                Debug.Log($"{name}: Player otomatik atandı.");
            }
            else
            {
                Debug.LogWarning($"{name}: Player bulunamadı! Tag’ini kontrol et.");
            }
        }

        
    }

    void Start()
    {
        // 3) Inspector atamalarını kontrol et:
        if (player == null) Debug.LogError($"{name}: Player Transform atanmamış!");
        if (ballPrefab == null) Debug.LogError($"{name}: Ball Prefab atanmamış!");
        if (handTransform == null) Debug.LogError($"{name}: Hand Transform atanmamış!");

        
    }

    void Update()
    {
        // 4) LookAtPlayer gerçekten çağrılıyor mu?
        if (player != null)
        {
            LookAtPlayer();
        }

        if (canMove)
            MoveSideToSide();
    }

    void LookAtPlayer()
    {
        // 5) Yatay düzlemde yön hesaplama
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
        {
            Debug.Log($"{name}: Player ile aynı pozisyondayım, dönme gereksiz.");
            return;
        }

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);

        // 6) Her frame logla
        Debug.Log($"{name} bakış açısı (Euler): {transform.eulerAngles}");
    }

    void MoveSideToSide()
    {
        Vector3 offset = Vector3.right * (movingRight ? 1 : -1) * moveSpeed * Time.deltaTime;
        transform.Translate(offset, Space.World);

        if (Vector3.Distance(transform.position, startPos) > moveRange)
            movingRight = !movingRight;
    }

    public void BeginThrow()
    {
        StartCoroutine(ThrowBallAfterDelay());
    }

    private IEnumerator ThrowBallAfterDelay()
    {
        canMove = false;
        float delay = Random.Range(0.2f, 1f);
        Debug.Log($"{name}: Throw gecikmesi {delay:F2}s");
        yield return new WaitForSeconds(delay);

        ThrowBall();

        yield return new WaitForSeconds(Random.Range(minHoldTime, maxHoldTime));
        canMove = true;
        
    }

    void ThrowBall()
    {
        if (player == null || ballPrefab == null || handTransform == null)
            return;

        GameObject ball = Instantiate(ballPrefab, handTransform.position, Quaternion.identity);
        Vector3 targetPos = player.position + Vector3.up * 1.2f;
        Vector3 dir = (targetPos - handTransform.position).normalized;

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError($"{name}: Ball prefab’da Rigidbody yok!");
            return;
        }

        rb.linearVelocity = dir * throwForce;
        Debug.Log($"{name} top fırlattı. Yön: {dir}, Hız: {throwForce}");
    }
}
