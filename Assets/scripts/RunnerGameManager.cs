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

    private float countdownTime = 2f; // Tur ba��na 40 saniye geri say�m
    private bool isCountingDown = false; // Geri say�m aktif mi?

    private RunnerController runnerController;

    public GameObject Drogba;
    public GameObject Alex;
    public GameObject Quaresma;

    public Vector3 EnemyLocation1, EnemyLocation2;



    void Start()
    {
        Time.timeScale = 0f;            // En ba�ta oyundaki her �ey durur
        runnerController = RunnerCharacter.GetComponent<RunnerController>();
        ShowRoundScreen(currentRound);  // �lk turda drogbalar�n ekran�n� sahneye getirir.


    }

    void Update()
    {

        //  E�er tur ekran� a��ksa ve R'ye bas�l�rsa yeni tur ba�lat
        if (roundFinished && Input.GetKeyDown(KeyCode.R))
        {
            StartNewRound();

        }


        if (isCountingDown)
        {
            HandleCountdown();  // D�zenli olarak geri say�m� kontrol eder
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
        Debug.Log("Show round screen �al��t�");
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
        RunnerCharacter.GetComponent<RunnerController>().enabled = false;  // Ara sahneler esnas�nda karakterin
                                                                           // hareketini engeller
    }

    private void StartNewRound()
    {

        roundFinished = false;

        //Yeniden Spawn olma
        Rigidbody rb = RunnerCharacter.GetComponent<Rigidbody>();
        rb.isKinematic = true; // Fiziksel tepkileri ge�ici olarak kapat
        RunnerCharacter.transform.position = new Vector3(101.203f, 175.326f, 142.89f);
        RunnerCharacter.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        rb.isKinematic = false; // Fizik motorunu tekrar etkinle�tir
        runnerController.lives = 1;

        DestroyBallsPerRound();

        Time.timeScale = 1f; // Oyunu ba�lat
        countdownTime =40f;
        isCountingDown = true;
        countdownText.gameObject.SetActive(true);   // Geri say�m UI'�n� etkinle�tirir
        RunnerCharacter.GetComponent<RunnerController>().enabled = true;
        runnerController.CanSlowTime = true;

        if (currentRoundScreen != null)
            currentRoundScreen.SetActive(false); // Tur ekran�n� kapat
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
            currentRound = 1; // E�er ba�ar�s�z olup R ye bast�ysa 1 numaral� drogba ara sahnesine geri d�n
            RestartGame();
        }
        else if (currentRound == 5)
        {
            //Oyunu bitirip bir daha R ye basarsa tekrar 1. sava�a d�ner
            isCountingDown = false;
            countdownText.gameObject.SetActive(false);


            RestartGame(); // Yeni tur ba�lat
        }

        runnerController.ResetDash();

    }

    private void DestroyBallsPerRound()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        // Her bir d��man� (0,0,0)'a ta�� ve yok et
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

        // Her bir d��man� (0,0,0)'a ta�� ve yok et
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
        270f, // �lk d��man sola bakacak
        90f   // �kinci d��man sa�a bakacak
        };

        for (int i = 0; i < positions.Length; i++)
        {
            Instantiate(enemyPrefab, positions[i], Quaternion.Euler(0, yRotations[i], 0));
        }
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


        ActualEnemy = "Drogba";
        /*
        if (currentRound == 5)
        {
            currentRound = 1; 
        }
        else
        {
            currentRound = 4;       // Ba�ar�s�z ekran�n� a�
        }
        */

        countdownTime = 2f;
        Start();
    }

    private void HandleCountdown()
    {
        countdownTime -= Time.deltaTime;
        countdownText.text = Mathf.Ceil(countdownTime).ToString(); // Geri say�m de�erini ekranda g�ster

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