using UnityEngine;
using System.Collections;


public class RedThrowerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform cameraTransform;
    public GameObject ballPrefab;
    public GameObject Drogba;
    public Transform throwPoint;
    public float throwForce = 10f;
    public float holdDistance = 1.5f;

    private GameObject heldBall;
    private Rigidbody rb;
    private float rotationX = 0f;
    private float rotationY = 0f;
    public float sensitivity = 2f;
    private GameObject nearbyBall;

    private bool canHold=false;
    public bool IsRedActive;

    private int ballCounter = 0;
    public int ballLimit = 10;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Invoke("spawnNewBall" , 3f);
        Invoke("canHoldCoolDown" , 2f);
        Invoke("spawnEnemy", 3f);
        
        
        
    }

    void Update()
    {
        // Eðer oyun duraklatýldýysa, karakter hareket etmeyecek.
        if (PauseMenu.gameIsPaused) return;  // Pause menüsü aktifse, hareket etme.

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
        CancelInvoke("SpawnNewBall");
        
    }

    void ThrowBall()
{
    if (IsRedActive)
    {
        
        heldBall.GetComponent<Rigidbody>().isKinematic = false;

        
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        Vector3 targetPoint;

        
        if (Physics.Raycast(ray, out hit, 40f))
        {
            targetPoint = hit.point;
        }
        else // Çarpmazsa ileri bir uzak nokta hedef olarak alýnýr
        {
            targetPoint = cameraTransform.position + cameraTransform.forward * 40f;
        }

        // Topun hedefe doðru yönünü alýyoruz
        Vector3 throwDirection = (targetPoint - heldBall.transform.position).normalized;

        // Fýrlatma kuvvetini o yöne uyguluyoruz
        heldBall.GetComponent<Rigidbody>().AddForce(throwDirection * throwForce, ForceMode.Impulse);

        // Topla ilgili kontrol deðiþkenleri güncelleniyor
        canHold = false;
        heldBall = null;
        Invoke("canHoldCoolDown", 2f);
        Invoke("spawnNewBall", 3f);
    }
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

    private void canHoldCoolDown()
    {
        
        canHold = true;
    }

    private void spawnNewBall()
    {
        if (ballCounter == ballLimit)
        {
            Debug.Log("Baþaramadýn");
        }
        else if (ballCounter < ballLimit)
        {
            Vector3 spawnPosition = transform.position - transform.forward * 2f;
            Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            ballCounter++;
        }

        
    }

    public void EnableControls(bool state)
    {
        this.enabled = state;
    }

    private void spawnEnemy()
    {
        
    }

    

}
