using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunnerGameManager : MonoBehaviour
{
    public GameObject RunnerCharacter;

    /*
    public GameObject Drogba;
    public GameObject Alex;
    public GameObject Quaresma;
   

    public Camera redCamera;
    public Camera blueCamera;
     */
    private bool IsGameStart;

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

    private float countdownTime = 2f; // Tur baþýna 40 saniye geri sayým
    private bool isCountingDown = false; // Geri sayým aktif mi?

    private RunnerController runnerController;

    public GameObject Drogba;
    public GameObject Alex;
    public GameObject Quaresma;

    public Vector3 EnemyLocation1, EnemyLocation2;



    void Start()
    {
        Time.timeScale = 0f;            // En baþta oyundaki her þey durur
        runnerController = RunnerCharacter.GetComponent<RunnerController>();
        ShowRoundScreen(currentRound);  // Ýlk turda drogbalarýn ekranýný sahneye getirir.


    }

    void Update()
    {

        //  Eðer tur ekraný açýksa ve R'ye basýlýrsa yeni tur baþlat
        if (roundFinished && Input.GetKeyDown(KeyCode.R))
        {
            StartNewRound();

        }


        if (isCountingDown)
        {
            HandleCountdown();  // Düzenli olarak geri sayýmý kontrol eder
        }

        if (!(runnerController.isAlive))
        {
            currentRound = 4;
            ShowRoundScreen(currentRound);
            Debug.Log("qqqqqqq");
        }



    }

    private void ShowRoundScreen(int round)
    {
        Debug.Log("Show round screen çalýþtý");
        roundFinished = true;
        runnerController.isAlive = true;
        Time.timeScale = 0f;
        isCountingDown = false;
        if (round == 1) currentRoundScreen = round1Screen;
        else if (round == 2) currentRoundScreen = round2Screen;
        else if (round == 3) currentRoundScreen = round3Screen;
        else if (round == 4) currentRoundScreen = FailScreen;
        else if (round == 5) currentRoundScreen = WinScreen;


        if (currentRoundScreen != null)
        {
            currentRoundScreen.SetActive(true);
        }
        RunnerCharacter.GetComponent<RunnerController>().enabled = false;  // Ara sahneler esnasýnda karakterin
                                                                           // hareketini engeller
    }

    private void StartNewRound()
    {

        roundFinished = false;

        //Yeniden Spawn olma
        Rigidbody rb = RunnerCharacter.GetComponent<Rigidbody>();
        rb.isKinematic = true; // Fiziksel tepkileri geçici olarak kapat
        RunnerCharacter.transform.position = new Vector3(101.203f, 175.326f, 142.89f);
        RunnerCharacter.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        rb.isKinematic = false; // Fizik motorunu tekrar etkinleþtir
        runnerController.lives = 1;

        DestroyBallsPerRound();

        Time.timeScale = 1f; // Oyunu baþlat
        countdownTime =40f;
        isCountingDown = true;
        countdownText.gameObject.SetActive(true);   // Geri sayým UI'ýný etkinleþtirir
        RunnerCharacter.GetComponent<RunnerController>().enabled = true;
        runnerController.CanSlowTime = true;

        if (currentRoundScreen != null)
            currentRoundScreen.SetActive(false); // Tur ekranýný kapat
        if (currentRoundScreen == WinScreen)
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            return;
        }


        if (currentRound == 1)
        {
            
            DrogbaSpawn();
            runnerController.sliderSpeed = 150f;
        }

        else if (currentRound == 2)
        {
            
            AlexSpawn();
            runnerController.sliderSpeed = 100f;
        }

        else if (currentRound == 3)
        {
            
            QuaresmaSpawn();
            runnerController.sliderSpeed = 70f;
        }

        else if (currentRound == 4)
        {
            currentRound = 1; // Eðer baþarýsýz olup R ye bastýysa 1 numaralý drogba ara sahnesine geri dön
            RestartGame();
        }
        else if (currentRound == 5)
        {
            //Oyunu bitirip bir daha R ye basarsa tekrar 1. savaþa döner
            isCountingDown = false;
            countdownText.gameObject.SetActive(false);


            RestartGame(); // Yeni tur baþlat
        }

        runnerController.ResetDash();

    }

    private void DestroyBallsPerRound()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        // Her bir düþmaný (0,0,0)'a taþý ve yok et
        foreach (GameObject ball in balls)
        {
            ball.transform.position = Vector3.zero;
            Destroy(ball);
        }
    }




    private void DrogbaSpawn()
    {
        ActualEnemy = "Drogba";
        SpawnEnemy(Drogba);
    }

    private void AlexSpawn()
    {
        ActualEnemy = "Alex";
        SpawnEnemy(Alex);
    }

    private void QuaresmaSpawn()
    {
        ActualEnemy = "Quaresma";
        SpawnEnemy(Quaresma);
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Her bir düþmaný (0,0,0)'a taþý ve yok et
        foreach (GameObject enemy in enemies)
        {
            enemy.transform.position = Vector3.zero;
            Destroy(enemy);
        }




        Vector3[] positions = new Vector3[]
        {
           EnemyLocation1, EnemyLocation2,
    };  

        float[] yRotations = new float[]
        {
        270f, // Ýlk düþman sola bakacak
        90f   // Ýkinci düþman saða bakacak
        };

        for (int i = 0; i < positions.Length; i++)
        {
            Instantiate(enemyPrefab, positions[i], Quaternion.Euler(0, yRotations[i], 0));
        }
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


        ActualEnemy = "Drogba";
        /*
        if (currentRound == 5)
        {
            currentRound = 1; 
        }
        else
        {
            currentRound = 4;       // Baþarýsýz ekranýný aç
        }
        */

        countdownTime = 2f;
        Start();
    }

    private void HandleCountdown()
    {
        countdownTime -= Time.deltaTime;
        countdownText.text = Mathf.Ceil(countdownTime).ToString(); // Geri sayým deðerini ekranda göster

        if (countdownTime <= 0)
        {
            Debug.Log("currentRound");
            if (currentRound <= 2)
            {
                currentRound++;
                ShowRoundScreen(currentRound);
            }
            else
            {
                currentRound = 5;
                ShowRoundScreen(currentRound);

            }



        }
    }


}