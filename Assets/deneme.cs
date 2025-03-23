using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnY : MonoBehaviour
{
    // Y tu�una bas�ld���nda hangi sahne y�klenecek
    public string sceneToLoad = "MainMenu";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
