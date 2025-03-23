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

    private bool canHold=true;

    
    public bool IsRedActive;

    private int ballCounter = 0;
    public int ballLimit = 5;

    public Image[] ballIcons; // UI'deki top simgeleri
    

    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        RedStartNewRound(); // Yeni bir tur baþlat
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

            canHold = false;
            Invoke("canHoldCoolDown", 2f);

            Vector3 throwDirection = (targetPoint - heldBall.transform.position).normalized;
            heldBall.GetComponent<Rigidbody>().AddForce(throwDirection * throwForce, ForceMode.Impulse);

            // UI güncellemesi
            if (ballCounter < ballIcons.Length)
            {
                ballIcons[ballCounter].gameObject.SetActive(false);
            }

            
            heldBall = null;
            ballCounter++;

            // Yeni bir top spawn etme iþlemi
            Invoke("spawnNewBall", 3f);
        }
    }

    void canHoldCoolDown()
    {
        canHold = true;


    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Ball"))
        {

            nearbyBall = other.gameObject;
            if (heldBall == null && canHold )
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

    

    private void spawnNewBall()
    {
        if (ballCounter <= ballLimit)
        {
            // Topu spawn etmeden önce pozisyonu kontrol et
            Vector3 spawnPosition = transform.position - cameraTransform.forward * 1f; // Kamera yönünde biraz öne
            Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("Baþaramadýn");
        }
    }

    // Yeni tur baþlatma fonksiyonu
    public void RedStartNewRound()
    {
        ballCounter = 0;
        Invoke("spawnNewBall", 1f); // 1 saniye gecikme ile topu spawn et

        for (int i = 0; i < ballLimit; i++)

        {
            ballIcons[i].gameObject.SetActive(true);
        }
    }
    

    public void EnableControls(bool state)
    {
        this.enabled = state;
    }
}
