using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RunnerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform cameraTransform;
    private Rigidbody rb;

    public BoxCollider catchZone;
    public Slider accuracySlider;
    public float sliderSpeed = 100f;
    private int sliderDirection = 1;

    private bool isSlow = false;

    private float rotationX = 0f;
    private float rotationY = 0f;
    public float sensitivity = 2f;

    public bool isAlive = true;
    public bool CanSlowTime = true;

    private bool isTurning = false;
    private Quaternion targetRotation;
    public float turnSpeed = 180f;

    private bool isSpeedBoosted = false;
    private float originalSpeed;

    public GameObject CilekliLink;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        rotationY = transform.eulerAngles.y; // Baþlangýç rotasyonunu al
    }

    void Update()
    {
        if (PauseMenu.gameIsPaused) return;

        Move();
        UpdateAccuracySlider();
        HandleTurn(); // Smooth dönüþ burada yapýlacak

        if (!isTurning)
        {
            RotateCamera(); // Dönüþ yapýlýyorsa mouse kontrolünü devre dýþý býrak
        }

        if (Input.GetMouseButtonDown(0))
        {
            CatchBall();
        }

        if (!isSlow && Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (CanSlowTime)
            {
                StartCoroutine(SlowTime());
                CanSlowTime = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && !isTurning)
        {
            rotationY += 180f;
            targetRotation = Quaternion.Euler(0f, rotationY, 0f);
            isTurning = true;
        }
    }

    void HandleTurn()
    {
        if (isTurning)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation;
                isTurning = false;
            }
        }
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = (transform.right * moveX + transform.forward * moveZ) * moveSpeed * Time.deltaTime;
        transform.position += move;
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        rotationY += mouseX;

        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

    private IEnumerator SlowTime()
    {
        isSlow = true;
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        isSlow = false;
    }

    private void CatchBall()
    {
        Collider[] hits = Physics.OverlapBox(catchZone.bounds.center,
                                             catchZone.bounds.extents,
                                             catchZone.transform.rotation);
        float chance = 1f - (Mathf.Abs(accuracySlider.value) / 100f);
        foreach (Collider col in hits)
        {
            if (col.CompareTag("Ball"))
            {
                if (Random.value < chance)
                {
                    Destroy(col.gameObject);
                }
                break;
            }
        }
    }

    void UpdateAccuracySlider()
    {
        if (accuracySlider == null) return;

        float newValue = accuracySlider.value + sliderDirection * sliderSpeed * Time.deltaTime;
        if (newValue >= 100)
        {
            newValue = 100;
            sliderDirection = -1;
        }
        else if (newValue <= -100)
        {
            newValue = -100;
            sliderDirection = 1;
        }

        accuracySlider.value = newValue;
        float colorIndex = Mathf.Abs(newValue) / 100f;
        Color newColor = Color.Lerp(Color.green, Color.red, colorIndex);
        ColorBlock cb = accuracySlider.colors;
        cb.normalColor = newColor;
        accuracySlider.colors = cb;
    }

    public void EnableControls(bool state)
    {
        this.enabled = state;
    }
        
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CilekliLink"))
        {
            if (!isSpeedBoosted)
            {
                StartCoroutine(SpeedBoost());
            }

            Destroy(other.gameObject); // Objeyi sahneden kaldýr
        }
    }

    // RunnerController.cs içinde class'ýn sonuna ekleyin:

    private void OnCollisionEnter(Collision collision)
    {
        // Eðer capsule collider'ýnýzla top çarpýþýrsa:
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Oyuncuyu öldür
            isAlive = false;
            Debug.Log("Oyuncu vuruldu, oyun bitti!");
            // Burada isAlive false ise Update/Move vb. devre dýþý kalacak þekilde kontrol ekleyebilirsiniz.
        }
    }

    private IEnumerator SpeedBoost()
    {

        isSpeedBoosted = true;
        originalSpeed = moveSpeed;
        moveSpeed *= 1.5f; // %30 hýz artýþý

        StartCoroutine(ShowSpeedBoostMessage());

        yield return new WaitForSeconds(5f); // 5 saniye bekle

        moveSpeed = originalSpeed;
        isSpeedBoosted = false;

    }
    private IEnumerator ShowSpeedBoostMessage()
    {
        CilekliLink.SetActive(true);
        yield return new WaitForSeconds(5f);
        CilekliLink.SetActive(false);
    }
}
