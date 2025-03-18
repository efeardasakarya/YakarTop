using NUnit.Framework.Constraints;
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

    private int DrogbaCounter=0;

    private int AlexCounter=0 ;

    private int QuaresmaCounter=0;

    private string ActualEnemy="Drogba";

    void Start()
    {
        
        Invoke("StartGame", 3f);
        Debug.Log("OyunBaþladý!");

        DrogbaCounter = 0;
        AlexCounter = 0;
        QuaresmaCounter = 0;
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
                Debug.Log(DrogbaCounter);
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
            Debug.Log("AAAAAAHHHHHHHHHHHH");
            if (ActualEnemy == "Drogba")
            {
                DrogbaCounter --;
                Debug.Log(DrogbaCounter);

                if (DrogbaCounter == 0)
                {
                    Debug.Log("DrogbaÖldü");
                    Invoke("AlexSpawn", 3f);

                }
            }




            if (ActualEnemy == "Alex")
            {
                AlexCounter -= 1;
                if (AlexCounter == 0)
                {
                    Invoke("QuaresmaSpawn", 3f);

                }
            }

            if (ActualEnemy == "Quaresma")
            {
                QuaresmaCounter -= 1;
                if (QuaresmaCounter == 0)
                {
                    ActualEnemy = "Null";

                    Debug.Log("Tebrikler");

                }
            }
        }
        
    }

    private void AlexSpawn()
    {

        ActualEnemy = "Alex";

        Instantiate(Alex, new Vector3(101.6f, 174.3f, 142.9f), Quaternion.Euler(0, 270, 0));

        Instantiate(Alex, new Vector3(105.6f, 174.3f, 142.9f), Quaternion.Euler(0, 270, 0));

        Instantiate(Alex, new Vector3(110.26f, 174.3f, 142.9f), Quaternion.Euler(0, 270, 0));

    }

    private void QuaresmaSpawn()
    {
        ActualEnemy = "Quaresma";

        Instantiate(Quaresma, new Vector3(101.6f, 174.3f, 142.9f), Quaternion.Euler(0, 270, 0));

        Instantiate(Quaresma, new Vector3(105.6f, 174.3f, 142.9f), Quaternion.Euler(0, 270, 0));

        Instantiate(Quaresma, new Vector3(110.26f, 174.3f, 142.9f), Quaternion.Euler(0, 270, 0));
    }


}
