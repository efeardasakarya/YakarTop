using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnY : MonoBehaviour
{
    // Y tuşuna basıldığında hangi sahne yüklenecek
    public string sceneToLoad = "MainMenu";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
