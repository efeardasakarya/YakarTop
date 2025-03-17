using UnityEngine;
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

    private bool canHold=false;
    public bool IsRedActive;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Invoke("spawnNewBall" , 3f);
        Invoke("canHoldCoolDown" , 2f);
        
        
    }

    void Update()
    {
        // Eðer oyun duraklatýldýysa, karakter hareket etmeyecek.
        if (PauseMenu.gameIsPaused) return;  // Pause menüsü aktifse, hareket etme.

        Move();
        RotateCamera();

        if (heldBall != null)
        {
            Vector3 holdPosition = cameraTransform.position + cameraTransform.forward * holdDistance;
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
            heldBall.GetComponent<Rigidbody>().AddForce(cameraTransform.forward * throwForce, ForceMode.Impulse);
            canHold = false;
            heldBall = null;
            Debug.Log("Fýrlat");
            Invoke("canHoldCoolDown", 2f);
            Invoke("spawnNewBall", 3f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Ball"))
        {
            Debug.Log("girdim");
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
        
        Vector3 spawnPosition = transform.position - transform.forward * 2f; 
        Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
    }

    public void EnableControls(bool state)
    {
        this.enabled = state;
    }

}
