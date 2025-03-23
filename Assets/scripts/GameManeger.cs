using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManeger : MonoBehaviour
{
    public GameObject redCharacter;
    public GameObject blueCharacter;

    public GameObject Drogba;
    public GameObject Alex;
    public GameObject Quaresma;

    public Camera redCamera;
    public Camera blueCamera;

    private bool IsGameStart;
    

    private int DrogbaCounter = 0;
    private int AlexCounter = 0;
    private int QuaresmaCounter = 0;

    
    private string ActualEnemy = "Drogba";   // Ýlk Drogba spawnlanýr


    public int currentRound = 1;     
    public int maxRounds = 3;
    private bool roundFinished = false;


    public GameObject round1Screen; // Drogbalarýn geleceði geçiþ ekraný 
    public GameObject round2Screen; // Alexlerin geleceði geçiþ ekraný
    public GameObject round3Screen; // Quaresmalarýn geleceði geçiþ ekraný
    public GameObject FailScreen; // Baþarýsýz olmasý durumunda çýkacak ekran
    public GameObject WinScreen; // 3 roundu tamamlayýp oyunu kazanmasý durumunda çýkacak ekran

    private GameObject currentRoundScreen;

    public TextMeshProUGUI countdownText;

    private float countdownTime = 40f; // Tur baþýna 40 saniye geri sayým
    private bool isCountingDown = false; // Geri sayým aktif mi?

    void Start()
    {
        Time.timeScale = 0f;            // En baþta oyundaki her þey durur
        ShowRoundScreen(currentRound);  // Ýlk turda drogbalarýn ekranýný sahneye getirir.
        
        
    }

    void Update()
    {
        //  Eðer tur ekraný açýksa ve R'ye basýlýrsa yeni tur baþlat
        if (roundFinished && Input.GetKeyDown(KeyCode.R))
        {
            StartNewRound();
            
        }

        if(isCountingDown)
        {
            HandleCountdown();  // Düzenli olarak geri sayýmý kontrol eder
        }
        
    }

    private void ShowRoundScreen(int round)
    {
        roundFinished = true;
        Time.timeScale = 0f; 

        if (round == 1) currentRoundScreen = round1Screen;
        else if (round == 2) currentRoundScreen = round2Screen;
        else if (round == 3) currentRoundScreen = round3Screen;
        else if (round == 4) currentRoundScreen = FailScreen;
        else if (round == 5) currentRoundScreen = WinScreen;
        

        if (currentRoundScreen != null)
        {
            currentRoundScreen.SetActive(true);
        }
        redCharacter.GetComponent<RedThrowerController>().enabled = false;  // Ara sahneler esnasýnda karakterin
                                                                            // hareketini engeller
    }

    private void StartNewRound()
    {
        roundFinished = false;
        Time.timeScale = 1f; // Oyunu baþlat
        countdownTime = 40f;    
        isCountingDown = true;
        countdownText.gameObject.SetActive(true);   // Geri sayým UI'ýný etkinleþtirir

        SetActiveCharacter(true);
        redCharacter.GetComponent<RedThrowerController>().RedStartNewRound(); // Top sayýsý gösteren UI'ý resetler

        if (currentRoundScreen != null)
            currentRoundScreen.SetActive(false); // Tur ekranýný kapat

        if (currentRound == 1) DrogbaSpawn();
        else if (currentRound == 2) AlexSpawn();
        else if (currentRound == 3) QuaresmaSpawn();
        else if(currentRound == 4)
        {
            currentRound = 1; // Eðer baþarýsýz olup R ye bastýysa 1 numaralý drogba ara sahnesine geri dön
            Start();
        }
        else if (currentRound == 5)
        {
            //Oyunu bitirip bir daha R ye basarsa tekrar 1. savaþa döner
            isCountingDown = false;
            countdownText.gameObject.SetActive(false); 

            
            RestartGame(); // Yeni tur baþlat
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if(ActualEnemy == "Drogba")
            {
                DrogbaCounter++;
            }

            if (ActualEnemy == "Alex")
            {
                AlexCounter++;
            }

            if (ActualEnemy == "Quaresma")
            {
                QuaresmaCounter++;
            }


        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (ActualEnemy == "Drogba")
            {
                DrogbaCounter--;
                if (DrogbaCounter == 0)
                {
                    currentRound++; // Current Round'u 2 yaparak Alex ekranýný çýkart
                    ShowRoundScreen(currentRound);
                }
            }
            else if (ActualEnemy == "Alex")
            {
                AlexCounter--;
                if (AlexCounter == 0)
                {
                    currentRound++; // Current Round'u 3 yaparak Quaresma ekranýný çýkart
                    ShowRoundScreen(currentRound);
                    
                }
            }
            else if (ActualEnemy == "Quaresma")
            {
                QuaresmaCounter--;
                if (QuaresmaCounter == 0)
                {
                    currentRound += 2; // Current Round'u 5 yaparak kazanma ekranýný çýkart
                    ShowRoundScreen(currentRound);

                     
                }
            }
        }
    }

    private void DrogbaSpawn()
    {
        ActualEnemy = "Drogba";
        SpawnEnemy(Drogba, ref DrogbaCounter);
    }

    private void AlexSpawn()
    {
        ActualEnemy = "Alex";
        SpawnEnemy(Alex, ref AlexCounter);
    }

    private void QuaresmaSpawn()
    {
        ActualEnemy = "Quaresma";
        SpawnEnemy(Quaresma, ref QuaresmaCounter);
    }

    private void SpawnEnemy(GameObject enemyPrefab, ref int counter)
    {
        Vector3[] positions = new Vector3[]
        {
            new Vector3(101.6f, 174.3f, 142.9f),
            new Vector3(105.6f, 174.3f, 142.9f),
            new Vector3(110.26f, 174.3f, 142.9f)
        };

        counter = 0;
        foreach (Vector3 pos in positions)
        {
            Instantiate(enemyPrefab, pos, Quaternion.Euler(0, 270, 0));
        }
    }

    void SetActiveCharacter(bool isRed)
    {
        redCharacter.GetComponent<RedThrowerController>().enabled = isRed;
        redCharacter.GetComponent<RedThrowerController>().IsRedActive = isRed;
        //blueCharacter.GetComponent<BlueThrowerController>().IsBlueActive = !isRed;

        redCamera.gameObject.SetActive(isRed);
        //blueCamera.gameObject.SetActive(!isRed);
    }

    private void RestartGame()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Her bir düþmaný (0,0,0)'a taþý ve yok et
        foreach (GameObject enemy in enemies)
        {
            enemy.transform.position = Vector3.zero; 
            Destroy(enemy); 
        }
        redCharacter.GetComponent<RedThrowerController>().RedStartNewRound();
        
        DrogbaCounter = 0;
        AlexCounter = 0;
        QuaresmaCounter = 0;
        ActualEnemy = "Drogba";
        currentRound = 4;       // Baþarýsýz ekranýný aç
        countdownTime = 40f;
        Start();
}

    private void HandleCountdown()
    {
        countdownTime -= Time.deltaTime;
        countdownText.text = Mathf.Ceil(countdownTime).ToString(); // Geri sayým deðerini ekranda göster

        if (countdownTime <= 0)
        {
            isCountingDown = false; 
            countdownText.gameObject.SetActive(false); 
            
            
            RestartGame(); // Yeni tur baþlat
        }
    }



}