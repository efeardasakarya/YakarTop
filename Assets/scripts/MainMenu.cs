using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class MainMenu : MonoBehaviour
{
    public AudioSource Theme;

    void Start()
    {
        
        // Müzik çalmýyorsa baþlat
        if (Theme != null && !Theme.isPlaying)
        {
            Theme.Play();
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void SetFullscreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }

    public void SetMusic(bool isMusic)
    {
        Theme.mute = !isMusic;
    }
}
