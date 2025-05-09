using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class RunnerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform cameraTransform;
    private Rigidbody rb;

    // Added: BoxCollider used as catch zone for balls
    public BoxCollider catchZone;

    // Added: Slider for catch accuracy
    public Slider accuracySlider;
    public float sliderSpeed = 100f;
    private int sliderDirection = 1;

    // Variables for slow-motion feature
    private bool isSlow = false;

    // Camera rotation vars
    private float rotationX = 0f;
    private float rotationY = 0f;
    public float sensitivity = 2f;

    public bool isAlive=true;

    public bool CanSlowTime = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (PauseMenu.gameIsPaused) return;

        Move();
        RotateCamera();
        UpdateAccuracySlider();

        // 2) Catch ball on left click / touch
        if (Input.GetMouseButtonDown(0))  // [MODIFIED]
        {
            CatchBall();  // [ADDED]
        }

        // 3) Slow motion on '1' key
        if (!isSlow && Input.GetKeyDown(KeyCode.Alpha1))  // [ADDED]
        {
            if (CanSlowTime)
            {
                StartCoroutine(SlowTime());
                CanSlowTime = false;
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
        // 1) Fare hareketini oku
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // 2) Dikey bakýþ açýsýný güncelle ve sýnýrla
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // 3) Yatay dönüþ açýsýný güncelle
        rotationY += mouseX;

        // 4) Karakteri yatayda döndür
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

        // 5) Kamerayý sadece dikeyde (pitch) eð
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }


    // Added: Coroutine for temporary slow-motion
    private IEnumerator SlowTime()
    {
        isSlow = true;                                 // [ADDED]
        Time.timeScale = 0.3f;                         // [ADDED]
        yield return new WaitForSecondsRealtime(3f);   // [ADDED]
        Time.timeScale = 1f;                           // [ADDED]
        isSlow = false;                                // [ADDED]
    }

    // Added: Catch logic
    private void CatchBall()
    {
        // Overlap within catchZone
        Collider[] hits = Physics.OverlapBox(catchZone.bounds.center,
                                             catchZone.bounds.extents,
                                             catchZone.transform.rotation);
        float chance = 1f - (Mathf.Abs(accuracySlider.value) / 100f);  // Higher when slider near zero
        foreach (Collider col in hits)
        {
            if (col.CompareTag("Ball"))
            {
                if (Random.value < chance)
                {
                    // Successful catch: deactivate or pick up ball
                    Destroy(col.gameObject);  // or implement pickup behavior
                }
                break;  // Only attempt one ball
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // No longer needed: catch in CatchBall(), remove old empty block
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
}
