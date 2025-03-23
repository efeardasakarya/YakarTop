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
    private GameObject heldBall;
    private Rigidbody rb;

    //Kamera de�i�kenleri
    private float rotationX = 0f;
    private float rotationY = 0f;
    public float sensitivity = 2f;

    private GameObject nearbyBall;
    private bool canHold=true;
    public bool IsRedActive; // Karakterin aktif olup olmad���n kontrol eden de�i�ken
                             // �leriki a�amalarda kar��ya bir tane daha at�c� eklenecektir

    // S�n�rl� say�da top atmak i�in
    public int ballCounter = 0;
    public int ballLimit = 5;

    public Image[] ballIcons; // UI'deki top simgeleri

    //Slider i�in 
    public Slider accuracySlider;
    public float sliderSpeed = 100f;
    private int sliderDirection = 1;




    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        RedStartNewRound(); // Yeni bir tur ba�lat
    }

    void Update()
    {
        if (PauseMenu.gameIsPaused) return;  // Oyun durduruldu�unda di�er fonksiyonlar� durdurur

        Move();
        RotateCamera();
        UpdateAccuracySlider();


        if (heldBall != null)
        {
            Vector3 holdPosition = throwPoint.position;
            heldBall.transform.position = holdPosition;
            heldBall.transform.rotation = cameraTransform.rotation;

            if (Input.GetMouseButtonDown(0)) // LMB ile top f�rlatma
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

    

    void ThrowBall()
    {
        if (IsRedActive && heldBall != null && ballCounter < ballLimit)
        {
            heldBall.GetComponent<Rigidbody>().isKinematic = false;

            Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
            RaycastHit hit;
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out hit, 40f))  // E�er ray 40 birimlik mesafeye �arpt�ysa oraya at
            {
                targetPoint = hit.point;
            }
            else                                    // 40 birimden fazlaysa kameran�n bakt��� y�ne do�ru f�rlat
            {
                targetPoint = cameraTransform.position + cameraTransform.forward * 40f;
            }

            canHold = false;                        // �leri at�ld��� esnada topun collidera girip ele gelmemesi i�in
            Invoke("canHoldCoolDown", 2f);          // ge�i�i olarak tutumay� kapat


            Vector3 throwDirection = (targetPoint - heldBall.transform.position).normalized;

            float inaccuracy = accuracySlider.value / 100f; // 0�a yak�nsa daha do�ru
            float maxAngle = 20f; // maksimum sapma a��s�
            float angleOffset = inaccuracy * maxAngle;
            Vector3 randomOffset = Quaternion.Euler(angleOffset, angleOffset, 0) * throwDirection;
            throwDirection = randomOffset.normalized;
            heldBall.GetComponent<Rigidbody>().AddForce(throwDirection * throwForce, ForceMode.Impulse);

            // Her top at�ld���nda UI'dan bir top eksilt
            if (ballCounter < ballIcons.Length)
            {
                ballIcons[ballCounter].gameObject.SetActive(false);
            }
            heldBall = null;
            ballCounter++;

            // 3 saniyw sonra yeni bir top spawn etme i�lemi
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

    public void PickUpBall(GameObject ball)
    {
        heldBall = ball;
        heldBall.GetComponent<Rigidbody>().isKinematic = true;
        nearbyBall = null;
        CancelInvoke("spawnNewBall"); // Top oyuncu taraf�ndan tutuldu�unda yeni top spawn etme
    }



    private void spawnNewBall()
    {
        if (ballCounter <= ballLimit)
        {
            // Topu spawn etmeden �nce pozisyonu kontrol et
            Vector3 spawnPosition = transform.position - cameraTransform.forward * 1f; // Kamera y�n�nde biraz �ne
            Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        }
        
    }

    // Yeni tur ba�latma fonksiyonu. Top say�s�n� s�f�rlar
    public void RedStartNewRound()
    {
        ballCounter = 0;
        Invoke("spawnNewBall" , 0.1f); // 0.1 saniye gecikme ile topu spawn et

        for (int i = 0; i < ballLimit; i++) // UI daki top ikonlar�n� geri getir

        {
            ballIcons[i].gameObject.SetActive(true);
        }
    }


    void UpdateAccuracySlider()
    {
        if (accuracySlider == null) return;

        float newValue = accuracySlider.value + sliderDirection * sliderSpeed * Time.deltaTime;

        if (newValue >= 100) // Slidera 100 e geldi�inde azalt
        {
            newValue = 100;
            sliderDirection = -1;
        }
        else if (newValue <= -100) // slider -100 e geldi�inde artt�r
        {
            newValue = -100;
            sliderDirection = 1;
        }

        accuracySlider.value = newValue;


        
        float colorIndex = Mathf.Abs(newValue) / 100f; // 0'a yak�nken 0, 100'e yak�nken 1
        Color newColor = Color.Lerp(Color.green, Color.red, colorIndex); //100 e yak�n k�rm�z� , 0 a

        // ColorBlock g�ncelle
        ColorBlock cb = accuracySlider.colors;
        cb.normalColor = newColor;
        accuracySlider.colors = cb;

    }





    public void EnableControls(bool state)
    {
        this.enabled = state;
    }
}
