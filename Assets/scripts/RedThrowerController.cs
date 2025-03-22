using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMesh Pro i�in gerekli namespace
using System.Collections;

public class RedThrowerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform cameraTransform;
    public GameObject ballPrefab;
    public Transform throwPoint;
    public float throwForce = 10f;
    public float holdDistance = 1.5f;

    private GameObject heldBall;
    private Rigidbody rb;
    private float rotationX = 0f;
    private float rotationY = 0f;
    public float sensitivity = 2f;
    private GameObject nearbyBall;

    private bool canHold = false;
    public bool IsRedActive;

    private int ballCounter = 0;
    public int ballLimit = 5;

    public Image[] ballIcons; // UI'deki top simgeleri
    public TextMeshProUGUI countdownText; // Geri say�m i�in TextMesh Pro

    private float countdownTime = 40f; // Tur ba��na 40 saniye geri say�m
    private bool isCountingDown = false; // Geri say�m aktif mi?

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        StartNewRound(); // Yeni bir tur ba�lat
    }

    void Update()
    {
        if (PauseMenu.gameIsPaused) return;

        Move();
        RotateCamera();

        if (heldBall != null)
        {
            Vector3 holdPosition = throwPoint.position;
            heldBall.transform.position = holdPosition;
            heldBall.transform.rotation = cameraTransform.rotation;

            if (Input.GetMouseButtonDown(0))
            {
                ThrowBall();
            }
        }

        if (isCountingDown)
        {
            HandleCountdown();
        }
    }

    void Move()
    {
        if (IsRedActive)
        {
            float moveInput = Input.GetAxis("Horizontal");
            Vector3 move = transform.right * moveInput * moveSpeed * Time.deltaTime;
            transform.position += move;
        }
    }

    void RotateCamera()
    {
        if (IsRedActive)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);
            rotationY += mouseX;

            cameraTransform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        }
    }

    public void PickUpBall(GameObject ball)
    {
        heldBall = ball;
        heldBall.GetComponent<Rigidbody>().isKinematic = true;
        nearbyBall = null;
        CancelInvoke("spawnNewBall");
    }

    void ThrowBall()
    {
        if (IsRedActive && heldBall != null && ballCounter < ballLimit)
        {
            heldBall.GetComponent<Rigidbody>().isKinematic = false;

            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            RaycastHit hit;
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out hit, 40f))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = cameraTransform.position + cameraTransform.forward * 40f;
            }

            Vector3 throwDirection = (targetPoint - heldBall.transform.position).normalized;
            heldBall.GetComponent<Rigidbody>().AddForce(throwDirection * throwForce, ForceMode.Impulse);

            // UI g�ncellemesi
            if (ballCounter < ballIcons.Length)
            {
                ballIcons[ballCounter].gameObject.SetActive(false);
            }

            canHold = false;
            heldBall = null;
            ballCounter++;

            // Yeni bir top spawn etme i�lemi
            Invoke("spawnNewBall", 3f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            nearbyBall = other.gameObject;
            if (heldBall == null && canHold)
            {
                PickUpBall(nearbyBall);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            nearbyBall = null;
        }
    }

    private void canHoldCoolDown()
    {
        canHold = true;
    }

    private void spawnNewBall()
    {
        if (ballCounter >= ballLimit)
        {
            Debug.Log("Ba�aramad�n"); // T�m toplar at�ld�ysa bir �eyler yapabilirsin
        }
        else
        {
            // Topu spawn etmeden �nce pozisyonu kontrol et
            Vector3 spawnPosition = transform.position - cameraTransform.forward * 2f; // Kamera y�n�nde biraz �ne
            Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // Yeni tur ba�latma fonksiyonu
    private void StartNewRound()
    {
        countdownTime = 40f; // Her tur i�in 40 saniye
        countdownText.gameObject.SetActive(true); // Geri say�m UI'yi aktif et
        isCountingDown = true; // Geri say�m ba�lat

        // Topu oyuncunun eline spawn et (1 saniye gecikme ile)
        Invoke("SpawnBallInHand", 1f); // 1 saniye gecikme ile topu spawn et
    }

    // Topu oyuncunun eline yerle�tir
    private void SpawnBallInHand()
    {
        // Topu oyuncunun eline yerle�tirecek �ekilde spawn et
        if (heldBall == null && ballCounter < ballLimit)
        {
            // Top spawn ediliyor
            Vector3 spawnPosition = throwPoint.position; // Topun oyuncunun eline yerle�tirilece�i pozisyon
            heldBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            heldBall.GetComponent<Rigidbody>().isKinematic = true; // Topu tutabilmek i�in kinematic yap�yoruz
        }
    }

    // Geri say�m y�netim fonksiyonu
    private void HandleCountdown()
    {
        countdownTime -= Time.deltaTime;
        countdownText.text = Mathf.Ceil(countdownTime).ToString(); // Geri say�m de�erini ekranda g�ster

        if (countdownTime <= 0)
        {
            isCountingDown = false; // Geri say�m bitti
            countdownText.gameObject.SetActive(false); // Geri say�m UI'yi gizle
            // Burada yeni tur ba�latma veya ba�ka i�lemler yap�labilir
            StartNewRound(); // Yeni tur ba�lat
        }
    }

    public void EnableControls(bool state)
    {
        this.enabled = state;
    }
}
