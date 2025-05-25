using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class PauseMenuThrower : MonoBehaviour
{
    public static bool gameIsPaused = false;

    [Header("UI Elements")]
    public GameObject pauseMenuPanel;
    public GameObject optionsMenu;
    public GameObject panel; 
    public AudioSource theme;
    public GameObject howToPlayMenu; 


 
    public GameObject timerObject; 
    public GameObject imageParentObject;
    public GameObject SliderObject;
    public GameObject CrosshairObject;


    public List<GameObject> gameUIElements = new List<GameObject>();

    private EventSystem eventSystem;

    void Start()
    {
        Time.timeScale = 1.0f;
        gameIsPaused = false;


        pauseMenuPanel.SetActive(false);
        optionsMenu.SetActive(false);
        panel.SetActive(false);


        eventSystem = EventSystem.current;


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape'e basýldý.");
            if (optionsMenu.activeSelf)
            {
                CloseOptions();
            }
            else if (howToPlayMenu.activeSelf)
            {
                CloseHowToPlay();
            }
            else
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        Debug.Log("TogglePause çaðrýldý.");
        if (gameIsPaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        Debug.Log("Resume çaðrýldý");
        pauseMenuPanel.SetActive(false);
        panel.SetActive(false);
        Time.timeScale = 1.0f;
        gameIsPaused = false;


        Object.FindFirstObjectByType<RedThrowerController>().EnableControls(true); 
        Debug.Log("hareket aktif");


        ToggleUIElements(true);
        Debug.Log("Ui gösterildi");


        timerObject.SetActive(true);
        imageParentObject.SetActive(true);
        Debug.Log("Image parent aktif edildi");
        SliderObject.SetActive(true);
        CrosshairObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        Debug.Log("Pause çaðrýldý");
        pauseMenuPanel.SetActive(true);
        panel.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

        ToggleUIElements(false);


        timerObject.SetActive(false);
        imageParentObject.SetActive(false);
        SliderObject.SetActive(false);
        CrosshairObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    private void ToggleUIElements(bool show)
    {
        foreach (GameObject uiElement in gameUIElements)
        {
            uiElement.SetActive(show);
        }
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1.0f; 
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowOptions()
    {
        pauseMenuPanel.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

    public void SetFullscreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }

    public void SetMusic(bool isMusic)
    {
        theme.mute = !isMusic;
    }
    public void CloseHowToPlay()
    {
        howToPlayMenu.SetActive(false);
        pauseMenuPanel.SetActive(true); 
    }

}