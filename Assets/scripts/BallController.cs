using UnityEngine;

public class BallController : MonoBehaviour

{
    public GameObject player;
    private RunnerController runnerController;

    void Start()
    {
        if (player != null)
        {
            runnerController = player.GetComponent<RunnerController>();
        }
        else
        {
            Debug.LogWarning("Player GameObject not assigned in BallController.");
        }
    }


    void Update()
    {
        
    }
    // BallController.cs

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground") )
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Runner"))
        {
            // Oyuncunun capsule collider'ýyla çarpýþtýk
            Destroy(gameObject);
            Debug.Log("Oyuncuya isabet etti, top yok oldu.");
        }
    }

}
