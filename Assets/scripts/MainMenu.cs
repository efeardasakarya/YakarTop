using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class MainMenu : MonoBehaviour
{
    public AudioSource Theme;

    void Start()
    {
        Debug.Log("start calisti");
        Time.timeScale = 1f;
        Canvas myCanvas = FindObjectOfType<Canvas>();
        if (myCanvas != null)
        {
            myCanvas.gameObject.SetActive(true);
        }
        if (Theme != null && !Theme.isPlaying)
        {
  
            Theme.Play();
        }
    }


    public void LoadThrowerMode()
    {
        SceneManager.LoadScene("ThrowerLevel");
    }

    public void LoadRunnerMode()
    {
        SceneManager.LoadScene("RunnerLevel");
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
