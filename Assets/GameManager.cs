using UnityEngine;

public class GameManeger : MonoBehaviour
{
    public GameObject redCharacter;
    public GameObject blueCharacter;
    public Camera redCamera;
    public Camera blueCamera;

    private bool IsGameStart;

    private bool isRedActive = true;

    void Start()
    {
        
        Invoke("StartGame", 3f);
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

        
}
