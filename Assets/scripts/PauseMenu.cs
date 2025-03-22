using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro; // TextMesh Pro için gerekli namespace
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    [Header("UI Elements")]
    public GameObject pauseMenuPanel;
    public GameObject optionsMenu;
    public GameObject panel; // Arka plan karartma paneli
    public AudioSource theme;

    // Gizlemek istediðiniz öðeler
    public GameObject timerObject; // TextMesh Pro bileþenini içeren Timer
    public GameObject imageParentObject; // Image bileþenlerini içeren parent GameObject

    // Tüm oyun UI öðelerini saklamak için liste
    public List<GameObject> gameUIElements = new List<GameObject>();

    private EventSystem eventSystem;

    void Start()
    {
        Time.timeScale = 1.0f;
        gameIsPaused = false;

        // Baþlangýçta menüleri kapat
        pauseMenuPanel.SetActive(false);
        optionsMenu.SetActive(false);
        panel.SetActive(false);

        // EventSystem'i al
        eventSystem = EventSystem.current;

        // Oyun UI öðelerini listeye ekle
        // Bu listeye, gizlenmesini istediðiniz tüm UI öðelerini manuel olarak ekleyebilirsiniz
        gameUIElements.AddRange(GameObject.FindGameObjectsWithTag("GameUI"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
        if (gameIsPaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
        panel.SetActive(false);
        Time.timeScale = 1.0f;
        gameIsPaused = false;

        // Karakter hareketlerini aktif et
        Object.FindFirstObjectByType<RedThrowerController>().EnableControls(true);  // Hareketi aktif et

        // Diðer UI öðelerini tekrar göster
        ToggleUIElements(true);

        // Gizlenen öðeleri geri getirme
        timerObject.SetActive(true);
        imageParentObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        panel.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

        // Karakter hareketlerini durdur
        Object.FindFirstObjectByType<RedThrowerController>().EnableControls(false);  // Hareketi durdur

        // Diðer UI öðelerini gizle
        ToggleUIElements(false);

        // Gizlemek istediðiniz öðeleri gizle
        timerObject.SetActive(false);
        imageParentObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Diðer UI öðelerini gizlemek ve göstermek için yardýmcý fonksiyon
    private void ToggleUIElements(bool show)
    {
        foreach (GameObject uiElement in gameUIElements)
        {
            uiElement.SetActive(show);
        }
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1.0f; // Yeni sahne yüklenmeden önce zamaný düzelt
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