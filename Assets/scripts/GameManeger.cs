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

    
    private string ActualEnemy = "Drogba";   // �lk Drogba spawnlan�r


    public int currentRound = 1;     
    public int maxRounds = 3;
    private bool roundFinished = false;


    public GameObject round1Screen; // Drogbalar�n gelece�i ge�i� ekran� 
    public GameObject round2Screen; // Alexlerin gelece�i ge�i� ekran�
    public GameObject round3Screen; // Quaresmalar�n gelece�i ge�i� ekran�
    public GameObject FailScreen; // Ba�ar�s�z olmas� durumunda ��kacak ekran
    public GameObject WinScreen; // 3 roundu tamamlay�p oyunu kazanmas� durumunda ��kacak ekran

    private GameObject currentRoundScreen;

    public TextMeshProUGUI countdownText;

    private float countdownTime = 40f; // Tur ba��na 40 saniye geri say�m
    private bool isCountingDown = false; // Geri say�m aktif mi?

    void Start()
    {
        Time.timeScale = 0f;            // En ba�ta oyundaki her �ey durur
        ShowRoundScreen(currentRound);  // �lk turda drogbalar�n ekran�n� sahneye getirir.
        
        
    }

    void Update()
    {
        //  E�er tur ekran� a��ksa ve R'ye bas�l�rsa yeni tur ba�lat
        if (roundFinished && Input.GetKeyDown(KeyCode.R))
        {
            StartNewRound();
            
        }

        if(isCountingDown)
        {
            HandleCountdown();  // D�zenli olarak geri say�m� kontrol eder
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
        redCharacter.GetComponent<RedThrowerController>().enabled = false;  // Ara sahneler esnas�nda karakterin
                                                                            // hareketini engeller
    }

    private void StartNewRound()
    {
        roundFinished = false;
        Time.timeScale = 1f; // Oyunu ba�lat
        countdownTime = 40f;    
        isCountingDown = true;
        countdownText.gameObject.SetActive(true);   // Geri say�m UI'�n� etkinle�tirir

        SetActiveCharacter(true);
        redCharacter.GetComponent<RedThrowerController>().RedStartNewRound(); // Top say�s� g�steren UI'� resetler

        if (currentRoundScreen != null)
            currentRoundScreen.SetActive(false); // Tur ekran�n� kapat

        if (currentRound == 1) DrogbaSpawn();
        else if (currentRound == 2) AlexSpawn();
        else if (currentRound == 3) QuaresmaSpawn();
        else if(currentRound == 4)
        {
            currentRound = 1; // E�er ba�ar�s�z olup R ye bast�ysa 1 numaral� drogba ara sahnesine geri d�n
            Start();
        }
        else if (currentRound == 5)
        {
            //Oyunu bitirip bir daha R ye basarsa tekrar 1. sava�a d�ner
            isCountingDown = false;
            countdownText.gameObject.SetActive(false); 

            
            RestartGame(); // Yeni tur ba�lat
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
                    currentRound++; // Current Round'u 2 yaparak Alex ekran�n� ��kart
                    ShowRoundScreen(currentRound);
                }
            }
            else if (ActualEnemy == "Alex")
            {
                AlexCounter--;
                if (AlexCounter == 0)
                {
                    currentRound++; // Current Round'u 3 yaparak Quaresma ekran�n� ��kart
                    ShowRoundScreen(currentRound);
                    
                }
            }
            else if (ActualEnemy == "Quaresma")
            {
                QuaresmaCounter--;
                if (QuaresmaCounter == 0)
                {
                    currentRound += 2; // Current Round'u 5 yaparak kazanma ekran�n� ��kart
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

        // Her bir d��man� (0,0,0)'a ta�� ve yok et
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
        currentRound = 4;       // Ba�ar�s�z ekran�n� a�
        countdownTime = 40f;
        Start();
}

    private void HandleCountdown()
    {
        countdownTime -= Time.deltaTime;
        countdownText.text = Mathf.Ceil(countdownTime).ToString(); // Geri say�m de�erini ekranda g�ster

        if (countdownTime <= 0)
        {
            isCountingDown = false; 
            countdownText.gameObject.SetActive(false); 
            
            
            RestartGame(); // Yeni tur ba�lat
        }
    }



}