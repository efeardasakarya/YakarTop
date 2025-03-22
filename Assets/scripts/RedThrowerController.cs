using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMesh Pro için gerekli namespace
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
    public TextMeshProUGUI countdownText; // Geri sayým için TextMesh Pro

    private float countdownTime = 40f; // Tur baþýna 40 saniye geri sayým
    private bool isCountingDown = false; // Geri sayým aktif mi?

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        StartNewRound(); // Yeni bir tur baþlat
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

            // UI güncellemesi
            if (ballCounter < ballIcons.Length)
            {
                ballIcons[ballCounter].gameObject.SetActive(false);
            }

            canHold = false;
            heldBall = null;
            ballCounter++;

            // Yeni bir top spawn etme iþlemi
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
            Debug.Log("Baþaramadýn"); // Tüm toplar atýldýysa bir þeyler yapabilirsin
        }
        else
        {
            // Topu spawn etmeden önce pozisyonu kontrol et
            Vector3 spawnPosition = transform.position - cameraTransform.forward * 2f; // Kamera yönünde biraz öne
            Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // Yeni tur baþlatma fonksiyonu
    private void StartNewRound()
    {
        countdownTime = 40f; // Her tur için 40 saniye
        countdownText.gameObject.SetActive(true); // Geri sayým UI'yi aktif et
        isCountingDown = true; // Geri sayým baþlat

        // Topu oyuncunun eline spawn et (1 saniye gecikme ile)
        Invoke("SpawnBallInHand", 1f); // 1 saniye gecikme ile topu spawn et
    }

    // Topu oyuncunun eline yerleþtir
    private void SpawnBallInHand()
    {
        // Topu oyuncunun eline yerleþtirecek þekilde spawn et
        if (heldBall == null && ballCounter < ballLimit)
        {
            // Top spawn ediliyor
            Vector3 spawnPosition = throwPoint.position; // Topun oyuncunun eline yerleþtirileceði pozisyon
            heldBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            heldBall.GetComponent<Rigidbody>().isKinematic = true; // Topu tutabilmek için kinematic yapýyoruz
        }
    }

    // Geri sayým yönetim fonksiyonu
    private void HandleCountdown()
    {
        countdownTime -= Time.deltaTime;
        countdownText.text = Mathf.Ceil(countdownTime).ToString(); // Geri sayým deðerini ekranda göster

        if (countdownTime <= 0)
        {
            isCountingDown = false; // Geri sayým bitti
            countdownText.gameObject.SetActive(false); // Geri sayým UI'yi gizle
            // Burada yeni tur baþlatma veya baþka iþlemler yapýlabilir
            StartNewRound(); // Yeni tur baþlat
        }
    }

    public void EnableControls(bool state)
    {
        this.enabled = state;
    }
}
