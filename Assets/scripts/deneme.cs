using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnY : MonoBehaviour
{
    // Y tuþuna basýldýðýnda hangi sahne yüklenecek
    public string sceneToLoad = "MainMenu";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
