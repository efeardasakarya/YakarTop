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
    private bool isRedActive = true;

    private int DrogbaCounter = 0;
    private int AlexCounter = 0;
    private int QuaresmaCounter = 0;

    private string ActualEnemy = "Drogba";

    void Start()
    {
        Invoke("StartGame", 3f);
        Debug.Log("Oyun Baþladý!");

        DrogbaCounter = 0;
        AlexCounter = 0;
        QuaresmaCounter = 0;

        // Baþlangýçta Drogba'larý spawn ediyoruz
        DrogbaSpawn();
    }

    void Update()
    {
        if (IsGameStart)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                isRedActive = !isRedActive;
                SetActiveCharacter(isRedActive);
            }
        }
        switch(ActualEnemy)
        {
            case "Drogba":
             
                break;

            case "Alex":

                break;

            case "Quaresma":

                break;
        }
        
    }

    void SetActiveCharacter(bool isRed)
    {
        redCharacter.GetComponent<RedThrowerController>().IsRedActive = isRed;
        blueCharacter.GetComponent<BlueThrowerController>().IsBlueActive = !isRed;

        redCamera.gameObject.SetActive(isRed);
        blueCamera.gameObject.SetActive(!isRed);
    }

    void StartGame()
    {
        IsGameStart = true;
        SetActiveCharacter(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (ActualEnemy == "Drogba")
            {
                DrogbaCounter++;
                Debug.Log("Drogba Enter:" );
            }
            else if (ActualEnemy == "Alex")
            {
                AlexCounter++;
                Debug.Log("Alex Enter:");
            }
            else if (ActualEnemy == "Quaresma")
            {
                QuaresmaCounter++;
                Debug.Log("Quaresma Enter:");
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
                Debug.Log("Drogba Exit: ");

                if (DrogbaCounter <= 0)
                {
                    Invoke("AlexSpawn", 3f);
                }
            }
            else if (ActualEnemy == "Alex")
            {
                AlexCounter--;
                Debug.Log("Alex Exit: ");

                if (AlexCounter <= 0)
                {
                    Invoke("QuaresmaSpawn", 3f);
                }
            }
            else if (ActualEnemy == "Quaresma")
            {
                QuaresmaCounter--;
                Debug.Log("Quaresma Exit: ");

                if (QuaresmaCounter <= 0)
                {
                    Debug.Log("Tebrikler kazandýnýz");
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

        counter = 0; // Spawn'dan önce sýfýrla, sonra OnTriggerEnter ile artar

        foreach (Vector3 pos in positions)
        {
            Instantiate(enemyPrefab, pos, Quaternion.Euler(0, 270, 0));
        }
    }
}
