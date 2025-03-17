using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic; // Bu sat�r� ekleyin



public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    [Header("UI Elements")]
    public GameObject pauseMenuPanel;
    public GameObject optionsMenu;
    public GameObject panel; // Arka plan karartma paneli
    public AudioSource theme;

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
            Debug.Log("Escape bas�ld�!");
            if (optionsMenu.activeSelf)
            {
                CloseOptions();
            }
            else
            {
                TogglePause();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Fareyi UI �zerinde bir elemana t�klay�p t�klamad���n� kontrol et
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Fare UI'ye t�klad�!");
            }
            else
            {
                Debug.Log("Fare UI d���nda bir yere t�klad�!");
            }

            // Raycast kontrol� yaparak UI elementine t�klan�p t�klanmad���n� kontrol et
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                // E�er UI elementlerine t�kland�ysa, hangi ��eye t�kland���n� logla
                Debug.Log(" UI'ye t�klad�n! T�klanan UI: " + results[0].gameObject.name);
            }
            else
            {
                Debug.Log(" UI alg�lanmad�! UI d���nda bir yere t�klad�n.");
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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
