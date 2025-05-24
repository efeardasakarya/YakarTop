using System.Collections;
using UnityEngine;

public class DodgeballEnemy : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject ballPrefab;
    public Transform handTransform;

    [Header("Movement")]
    public float moveRange = 3f;
    public float moveSpeed = 2f;

    [Header("Throwing")]
    public float throwForce = 15f;
    private float minHoldTime = 0.3f;
    private float maxHoldTime = 0.8f;
    public float spawnDelayMin = 0.2f;
    public float spawnDelayMax = 1f;

    private Vector3 startPos;
    private bool movingRight = true;
    private bool canMove = true;

    void Awake()
    {
        startPos = transform.position;
        if (player == null)
        {
            var pObj = GameObject.FindGameObjectWithTag("Runner");
            if (pObj != null) player = pObj.transform;
            else Debug.LogWarning($"{name}: Player bulunamadı! Tag’ini kontrol et.");
        }
    }

    void Start()
    {
        if (player == null) Debug.LogError($"{name}: Player Transform atanmamış!");
        if (ballPrefab == null) Debug.LogError($"{name}: Ball Prefab atanmamış!");
        if (handTransform == null) Debug.LogError($"{name}: Hand Transform atanmamış!");
    }

    void Update()
    {
        if (player != null) LookAtPlayer();
        if (canMove) MoveSideToSide();
    }

    void LookAtPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f) return;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
    }

    void MoveSideToSide()
    {
        Vector3 offset = Vector3.forward * (movingRight ? 1 : -1) * moveSpeed * Time.deltaTime;
        transform.Translate(offset, Space.World);
        if (Vector3.Distance(transform.position, startPos) > moveRange) movingRight = !movingRight;
    }

    public void BeginThrow()
    {
        StartCoroutine(SpawnAndHoldThenThrow());
    }

    private IEnumerator SpawnAndHoldThenThrow()
    {
        canMove = false;

        // 1) Spawn gecikmesi
        float spawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);
        yield return new WaitForSeconds(spawnDelay);

        if (ballPrefab == null || handTransform == null)
            yield break;

        // 2) Topu el pozisyonunda üret ve parent yap
        GameObject ball = Instantiate(ballPrefab, handTransform.position, handTransform.rotation);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;       // Fizik etkilerini kapat
            rb.useGravity = false;
        }
        ball.transform.SetParent(handTransform);
        ball.transform.localPosition = Vector3.zero;
        ball.transform.localRotation = Quaternion.identity;

        // 3) Hold süresi
        float holdTime = Random.Range(minHoldTime, maxHoldTime);
        yield return new WaitForSeconds(holdTime);

        // 4) Parent'tan çıkar ve fiziksel hale getir
        ball.transform.SetParent(null);
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            Vector3 targetPos = player.position + Vector3.up * 1.2f;
            Vector3 dir = (targetPos - handTransform.position).normalized;
            rb.linearVelocity = dir * throwForce;
        }

        // 5) Tekrar hareket izni
        yield return new WaitForSeconds(Random.Range(minHoldTime, maxHoldTime));
        canMove = true;
    }
}
