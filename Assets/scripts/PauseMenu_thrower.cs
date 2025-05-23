using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro; // TextMesh Pro i�in gerekli namespace
using System.Collections.Generic;

public class PauseMenuThrower : MonoBehaviour
{
    public static bool gameIsPaused = false;

    [Header("UI Elements")]
    public GameObject pauseMenuPanel;
    public GameObject optionsMenu;
    public GameObject panel; // Arka plan karartma paneli
    public AudioSource theme;

    // Gizlemek istedi�iniz ��eler
    public GameObject timerObject; // TextMesh Pro bile�enini i�eren Timer
    public GameObject imageParentObject; // Image bile�enlerini i�eren parent GameObject
    public GameObject SliderObject;
    public GameObject CrosshairObject;

    // T�m oyun UI ��elerini saklamak i�in liste
    public List<GameObject> gameUIElements = new List<GameObject>();

    private EventSystem eventSystem;

    void Start()
    {
        Time.timeScale = 1.0f;
        gameIsPaused = false;

        // Ba�lang��ta men�leri kapat
        pauseMenuPanel.SetActive(false);
        optionsMenu.SetActive(false);
        panel.SetActive(false);

        // EventSystem'i al
        eventSystem = EventSystem.current;


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape'e bas�ld�.");
            if (optionsMenu.activeSelf)
            {
                CloseOptions();
            }
            else
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        Debug.Log("TogglePause �a�r�ld�.");
        if (gameIsPaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        Debug.Log("Resume �a�r�ld�");
        pauseMenuPanel.SetActive(false);
        panel.SetActive(false);
        Time.timeScale = 1.0f;
        gameIsPaused = false;

        // Karakter hareketlerini aktif et
        Object.FindFirstObjectByType<RedThrowerController>().EnableControls(true);  // Hareketi aktif et
        Debug.Log("hareket aktif");

        // Di�er UI ��elerini tekrar g�ster
        ToggleUIElements(true);
        Debug.Log("Ui g�sterildi");

        // Gizlenen ��eleri geri getirme
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
        Debug.Log("Pause �a�r�ld�");
        pauseMenuPanel.SetActive(true);
        panel.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

        // Karakter hareketlerini durdur

        // !!!!!!!!!!!!!!!!!!!!
        // �uanda sadece RedThrower i�in �al���yor. Ka�an b�l�mlerinde ka�an i�in �al��mal�. Ka�an�n Controller�nda ayn� isme sahip
        // ba�ka bir fonksiyon var

        //Object.FindFirstObjectByType<RedThrowerController>().EnableControls(false);  // Hareketi durdur

        // Di�er UI ��elerini gizle
        ToggleUIElements(false);

        // Gizlemek istedi�iniz ��eleri gizle
        timerObject.SetActive(false);
        imageParentObject.SetActive(false);
        SliderObject.SetActive(false);
        CrosshairObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Di�er UI ��elerini gizlemek ve g�stermek i�in yard�mc� fonksiyon
    private void ToggleUIElements(bool show)
    {
        foreach (GameObject uiElement in gameUIElements)
        {
            uiElement.SetActive(show);
        }
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1.0f; // Yeni sahne y�klenmeden �nce zaman� d�zelt
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
}