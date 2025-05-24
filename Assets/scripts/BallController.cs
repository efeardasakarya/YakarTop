using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    public GameObject player;
    private RunnerController runnerController;
    private string sceneName;
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (player != null)
        {
            runnerController = player.GetComponent<RunnerController>();
        }
        else
        {
            Debug.LogWarning("Player GameObject not assigned in BallController.");
        }
    }

    private void Update()
    {
        Debug.Log(sceneName);
    }

    private void OnCollisionEnter(Collision collision)
    {



        if (sceneName == "ThrowerLevel" && collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            Debug.Log("ThrowerLevel: Enemy'ye çarptý, top yok oldu.");
        }


        // Normal yok etme koþullarý
        else if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
            return;
        }

        else if (collision.gameObject.CompareTag("Runner"))
        {
            Destroy(gameObject);
            Debug.Log("Oyuncuya isabet etti, top yok oldu.");
            return;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        // Sahne ThrowerLevel ve tag “Enemy” ise yok et
        if (sceneName == "ThrowerLevel" && other.CompareTag("Enemy"))
        {
            Debug.Log("Trigger ile Enemy'ye çarptý, yok oluyor.");
            Destroy(gameObject);
            return;
        }
    }

}
